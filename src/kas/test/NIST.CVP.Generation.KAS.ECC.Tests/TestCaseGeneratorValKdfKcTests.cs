using System.Collections.Generic;
using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.NoKC;
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
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorValKdfKcTests
    {
        private readonly HashFunction _hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
        private readonly Mock<IDsaEccFactory> _dsaFactory = new Mock<IDsaEccFactory>();
        private readonly Mock<IDsaEcc> _dsa = new Mock<IDsaEcc>();
        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        private TestCaseGeneratorValKdfKc _subject;
        private IEntropyProvider _entropyProvider;
        private IEntropyProviderFactory _entropyProviderFactory;
        private IKasBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _kasBuilder;
        private ISchemeBuilder<KasDsaAlgoAttributesEcc, OtherPartySharedInformation<EccDomainParameters, EccKeyPair>, EccDomainParameters, EccKeyPair> _schemeBuilder;
        private IMacParametersBuilder _macParametersBuilder;
        private IKeyConfirmationFactory _keyConfirmationFactory;
        private INoKeyConfirmationFactory _noKeyConfirmationFactory;
        private IShaFactory _shaFactory;
        private IKdfFactory _kdfFactory;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new ShaFactory();

            _dsaFactory.Setup(s => s.GetInstance(It.IsAny<HashFunction>(), EntropyProviderTypes.Random))
                .Returns(_dsa.Object);

            _dsa
                .Setup(s => s.Sha)
                .Returns(_shaFactory.GetShaInstance(
                    new HashFunction(ModeValues.SHA2, DigestSizes.d224)
                ));
            _dsa.Setup(s => s.GenerateKeyPair(It.IsAny<EccDomainParameters>()))
                .Returns(new EccKeyPairGenerateResult(new EccKeyPair(new EccPoint(1, 2), 3)));

            _entropyProvider = new EntropyProvider(new Random800_90());
            _entropyProviderFactory = new EntropyProviderFactory();
            _schemeBuilder = new SchemeBuilderEcc(
                _dsaFactory.Object,
                _curveFactory,
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
            _keyConfirmationFactory = new KeyConfirmationFactory();
            _noKeyConfirmationFactory = new NoKeyConfirmationFactory();
            _kdfFactory = new FakeKdfFactory_BadDkm(_shaFactory);

            _subject = new TestCaseGeneratorValKdfKc(
                _curveFactory,
                _kasBuilder,
                _schemeBuilder,
                _shaFactory,
                _entropyProviderFactory,
                _macParametersBuilder,
                _kdfFactory,
                _keyConfirmationFactory,
                _noKeyConfirmationFactory,
                new List<TestCaseDispositionOption>() { TestCaseDispositionOption.Success }
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
            BuildTestGroup(scheme, testGroupIutRole, kcRole, kcType, macType, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

            if (macType == KeyAgreementMacType.AesCcm)
            {
                Assert.IsTrue(resultTestCase.NonceAesCcm.BitLength != 0, nameof(resultTestCase.NonceAesCcm));
            }
            else
            {
                Assert.IsNull(resultTestCase.NonceAesCcm, nameof(resultTestCase.NonceAesCcm));
            }

            #region key/nonce checks
            // server checks
            if (serverKeyGenRequirements.GeneratesStaticKeyPair)
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

            if (serverKeyGenRequirements.GeneratesEphemeralKeyPair)
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer != 0,
                    nameof(resultTestCase.EphemeralPrivateKeyServer));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerX != 0,
                    nameof(resultTestCase.EphemeralPublicKeyServerX));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServerY != 0,
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

            if (serverKeyGenRequirements.GeneratesDkmNonce)
            {
                Assert.IsTrue(resultTestCase.DkmNonceServer != null,
                    nameof(resultTestCase.DkmNonceServer));
            }
            else
            {
                Assert.IsTrue(resultTestCase.DkmNonceServer == null,
                    nameof(resultTestCase.DkmNonceServer));
            }

            if (serverKeyGenRequirements.GeneratesEphemeralNonce)
            {
                Assert.IsTrue(resultTestCase.EphemeralNonceServer != null,
                    nameof(resultTestCase.EphemeralNonceServer));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralNonceServer == null,
                    nameof(resultTestCase.EphemeralNonceServer));
            }

            // Iut checks
            if (iutKeyGenRequirements.GeneratesStaticKeyPair)
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyIut != 0,
                    nameof(resultTestCase.StaticPrivateKeyIut));
                Assert.IsTrue(resultTestCase.StaticPublicKeyIutX != 0,
                    nameof(resultTestCase.StaticPublicKeyIutX));
                Assert.IsTrue(resultTestCase.StaticPublicKeyIutY != 0,
                    nameof(resultTestCase.StaticPublicKeyIutY));
            }
            else
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyIut == 0,
                    nameof(resultTestCase.StaticPrivateKeyIut));
                Assert.IsTrue(resultTestCase.StaticPublicKeyIutX == 0,
                    nameof(resultTestCase.StaticPublicKeyIutX));
                Assert.IsTrue(resultTestCase.StaticPublicKeyIutY == 0,
                    nameof(resultTestCase.StaticPublicKeyIutY));
            }

            if (iutKeyGenRequirements.GeneratesEphemeralKeyPair)
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut != 0,
                    nameof(resultTestCase.EphemeralPrivateKeyIut));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutX != 0,
                    nameof(resultTestCase.EphemeralPublicKeyIutX));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutY != 0,
                    nameof(resultTestCase.EphemeralPublicKeyIutY));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut == 0,
                    nameof(resultTestCase.EphemeralPrivateKeyIut));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutX == 0,
                    nameof(resultTestCase.EphemeralPublicKeyIutX));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyIutY == 0,
                    nameof(resultTestCase.EphemeralPublicKeyIutY));
            }

            if (iutKeyGenRequirements.GeneratesDkmNonce)
            {
                Assert.IsTrue(resultTestCase.DkmNonceIut != null,
                    nameof(resultTestCase.DkmNonceIut));
            }
            else
            {
                Assert.IsTrue(resultTestCase.DkmNonceIut == null,
                    nameof(resultTestCase.DkmNonceIut));
            }

            if (iutKeyGenRequirements.GeneratesEphemeralNonce)
            {
                Assert.IsTrue(resultTestCase.EphemeralNonceIut != null,
                    nameof(resultTestCase.EphemeralNonceIut));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralNonceIut == null,
                    nameof(resultTestCase.EphemeralNonceIut));
            }
            #endregion key/nonce checks
        }

        [Test]
        #region FullUnified
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        #endregion FullUnified

        #region FullMqv
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.FullMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        #endregion FullMqv

        #region OnePassUnified
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        #endregion OnePassUnified

        #region OnePassMqv
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassMqv, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        #endregion OnePassMqv

        #region OnePassDh
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        //[TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.OnePassDh, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        #endregion OnePassDh

        #region StaticUnified
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]

        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.Success, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)] // does not generate ephemeral key
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, false)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedOi, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedDkm, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedMacData, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(EccScheme.StaticUnified, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224, TestCaseDispositionOption.FailChangedTag, true)]
        #endregion StaticUnified
        public void ShouldSetProperTestCaseFailureTestProperty(
            EccScheme scheme,
            KeyConfirmationRole kcRole,
            KeyConfirmationDirection kcType,
            KeyAgreementRole testGroupIutRole,
            KeyAgreementMacType macType,
            TestCaseDispositionOption option,
            bool isFailure
        )
        {
            _subject = new TestCaseGeneratorValKdfKc(
                _curveFactory,
                _kasBuilder,
                _schemeBuilder,
                _shaFactory,
                _entropyProviderFactory,
                _macParametersBuilder,
                _kdfFactory,
                _keyConfirmationFactory,
                _noKeyConfirmationFactory,
                new List<TestCaseDispositionOption> { option }
            );

            BuildTestGroup(scheme, testGroupIutRole, kcRole, kcType, macType, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

            Assert.AreEqual(isFailure, resultTestCase.FailureTest);
        }

        private void BuildTestGroup(
            EccScheme scheme,
            KeyAgreementRole testGroupIutRole,
            KeyConfirmationRole kcRole,
            KeyConfirmationDirection kcType,
            KeyAgreementMacType macType,
            out SchemeKeyNonceGenRequirement<EccScheme> iutKeyGenRequirements,
            out SchemeKeyNonceGenRequirement<EccScheme> serverKeyGenRequirements,
            out TestCase resultTestCase
        )
        {
            TestGroup tg = new TestGroup()
            {
                KasMode = KasMode.KdfKc,
                Scheme = scheme,
                KasRole = testGroupIutRole,
                Function = KasAssurance.None,
                HashAlg = _hashFunction,
                KcRole = kcRole,
                KcType = kcType,
                MacType = macType,
                TestType = "VAL",
                Curve = Curve.B163,
                KeyLen = 128,
                MacLen = 128,
                KdfType = "concatenation",
                OiPattern = "uPartyInfo||vPartyInfo",
                IdIut = new BitString("a1b2c3d4e5"),
                IdServer = new BitString("434156536964")
            };

            tg.IdIutLen = tg.IdIut.BitLength;
            tg.IdServerLen = tg.IdServer.BitLength;

            if (macType == KeyAgreementMacType.AesCcm)
            {
                tg.AesCcmNonceLen = 104;
            }

            KeyAgreementRole serverRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyAgreementRole(testGroupIutRole);
            KeyConfirmationRole serverKeyConfRole =
                KeyGenerationRequirementsHelper.GetOtherPartyKeyConfirmationRole(tg.KcRole);

            iutKeyGenRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                scheme,
                tg.KasMode,
                tg.KasRole,
                tg.KcRole,
                tg.KcType
            );
            serverKeyGenRequirements = KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                scheme,
                tg.KasMode,
                serverRole,
                serverKeyConfRole,
                tg.KcType
            );

            var result = _subject.Generate(tg, false);
            resultTestCase = (TestCase)result.TestCase;
        }
    }
}