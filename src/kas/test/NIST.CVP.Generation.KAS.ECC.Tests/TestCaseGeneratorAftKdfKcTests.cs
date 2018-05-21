using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme.Ecc;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAftKdfKcTests
    {
        private TestCaseGeneratorAftKdfKc _subject;

        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        private Mock<IDsaEcc> _dsa;
        private IEntropyProvider _entropyProvider;
        private IEntropyProviderFactory _entropyProviderFactory;
        private IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        private ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        private Mock<IDsaEccFactory> _dsaFactory;
        private IMacParametersBuilder _macParametersBuilder;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaEcc>();
            _dsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<EccDomainParametersGenerateRequest>()))
                .Returns(
                    new EccDomainParametersGenerateResult(
                        
                    )
                );
            _dsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(
                    new EccKeyPairGenerateResult(
                        new EccKeyPair(new EccPoint(3, 4), 5)
                    )
                );
            _dsaFactory = new Mock<IDsaEccFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);

            _entropyProvider = new EntropyProvider(new Random800_90());
            _entropyProviderFactory = new EntropyProviderFactory();
            _schemeBuilder = new SchemeBuilderEcc(
                    _dsaFactory.Object,
                    new EccCurveFactory(), 
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(_entropyProvider),
                    _entropyProvider,
                    new DiffieHellmanEcc(),
                    new MqvEcc()
            );
            _kasBuilder = new KasBuilderEcc(_schemeBuilder);

            _macParametersBuilder = new MacParametersBuilder();

            _subject = new TestCaseGeneratorAftKdfKc(
                _curveFactory, _kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder
            );
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        // Note EphemeralUnified does not support key confirmation, commented out to show gaps in comparison to other schemes
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.EphemeralUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        // NOTE DhOneFlow allows for unilateral KeyConfirmation from V to U only
        // commented out testcases are to show the "gaps" in comparison to other schemes
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]
        public void ShouldPopulateCorrectKeysNoncesForSchemeRole(
            EccScheme scheme,
            KeyConfirmationRole kcRole,
            KeyConfirmationDirection kcType,
            KeyAgreementRole testGroupIutRole,
            KeyAgreementMacType macType
        )
        {
            TestGroup tg = new TestGroup()
            {
                KasMode = KasMode.KdfKc,
                Scheme = scheme,
                KasRole = testGroupIutRole,
                Function = KasAssurance.None,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                KcRole = kcRole,
                KcType = kcType,
                MacType = macType,
                KeyLen = 128,
                MacLen = 128,
                KdfType = "concatenation",
                OiPattern = "uPartyInfo||vPartyInfo",
                TestType = "AFT",
                Curve = Curve.B163
            };

            if (macType == KeyAgreementMacType.AesCcm)
            {
                tg.AesCcmNonceLen = 128;
            }

            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroupIutRole);
            KeyConfirmationRole serverKeyConfRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(tg.KcRole);

            var keyGenRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                scheme,
                tg.KasMode,
                serverRole,
                serverKeyConfRole,
                tg.KcType
            );

            var result = _subject.Generate(tg, false);
            var resultTestCase = (TestCase)result.TestCase;

            Assert.IsTrue(resultTestCase.Deferred, nameof(resultTestCase.Deferred));

            if (macType == KeyAgreementMacType.AesCcm && serverRole == KeyAgreementRole.InitiatorPartyU)
            {
                Assert.IsTrue(resultTestCase.NonceAesCcm.BitLength != 0, nameof(resultTestCase.NonceAesCcm));
            }
            else
            {
                Assert.IsNull(resultTestCase.NonceAesCcm, nameof(resultTestCase.NonceAesCcm));
            }

            if (keyGenRequirements.GeneratesStaticKeyPair)
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyServer != 0,
                    nameof(resultTestCase.StaticPrivateKeyServer));
                Assert.IsTrue(resultTestCase.StaticPublicKeyServerX != 0,
                    nameof(resultTestCase.StaticPublicKeyServerX));
                Assert.IsTrue(resultTestCase.StaticPublicKeyServerY != 0,
                    nameof(resultTestCase.StaticPublicKeyServerY));
            }
            else
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyServer == 0,
                    nameof(resultTestCase.StaticPrivateKeyServer));
                Assert.IsTrue(resultTestCase.StaticPublicKeyServerX == 0,
                    nameof(resultTestCase.StaticPublicKeyServerX));
                Assert.IsTrue(resultTestCase.StaticPublicKeyServerY == 0,
                    nameof(resultTestCase.StaticPublicKeyServerY));
            }

            if (keyGenRequirements.GeneratesEphemeralKeyPair)
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer != 0,
                    nameof(resultTestCase.EphemeralPrivateKeyServer));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerX != 0,
                    nameof(resultTestCase.EphemeralPublicKeyServerY));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerX != 0,
                    nameof(resultTestCase.EphemeralPublicKeyServerY));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer == 0,
                    nameof(resultTestCase.EphemeralPrivateKeyServer));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerX == 0,
                    nameof(resultTestCase.EphemeralPublicKeyServerX));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerY == 0,
                    nameof(resultTestCase.EphemeralPublicKeyServerY));
            }

            if (keyGenRequirements.GeneratesDkmNonce)
            {
                Assert.IsTrue(resultTestCase.DkmNonceServer != null,
                    nameof(resultTestCase.DkmNonceServer));
            }
            else
            {
                Assert.IsTrue(resultTestCase.DkmNonceServer == null,
                    nameof(resultTestCase.DkmNonceServer));
            }

            if (keyGenRequirements.GeneratesEphemeralNonce)
            {
                Assert.IsTrue(resultTestCase.EphemeralNonceServer != null,
                    nameof(resultTestCase.EphemeralNonceServer));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralNonceServer == null,
                    nameof(resultTestCase.EphemeralNonceServer));
            }
        }
    }
}