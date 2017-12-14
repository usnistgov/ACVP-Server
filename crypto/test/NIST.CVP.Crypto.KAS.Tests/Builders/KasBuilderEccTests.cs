using Moq;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC.Enums;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ecc;
using NIST.CVP.Crypto.KAS.Enums;
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

namespace NIST.CVP.Crypto.KAS.Tests.Builders
{
    [TestFixture, FastCryptoTest]
    public class KasBuilderEccTests
    {
        private readonly IEccCurveFactory _curveFactory = new EccCurveFactory();

        private KasBuilderEcc _subject;

        private Curve _curve;
        private MacParametersBuilder _macParamsBuilder;
        private Mock<IDsaEcc> _dsa;
        private Mock<IDsaEccFactory> _dsaFactory;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _curve = Curve.B163;

            _dsa = new Mock<IDsaEcc>();
            _dsaFactory = new Mock<IDsaEccFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
            _entropyProviderScheme = new TestableEntropyProvider();
            _entropyProviderOtherInfo = new TestableEntropyProvider();

            _subject = new KasBuilderEcc(
                new SchemeBuilderEcc(
                    _dsaFactory.Object,
                    _curveFactory,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(_entropyProviderOtherInfo),
                    _entropyProviderScheme,
                    new DiffieHellmanEcc(),
                    new MqvEcc()
                )
            );

            _macParamsBuilder = new MacParametersBuilder();
        }

        [Test]
        public void ShouldReturnComponentOnlyKas()
        {
            var result = _subject
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(EccScheme.OnePassMqv, EccParameterSet.Eb, _curve))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildNoKdfNoKc()
                .Build();


            Assert.AreEqual(KasMode.NoKdfNoKc, result.Scheme.SchemeParameters.KasMode);
        }

        [Test]
        public void ShouldReturnNoKeyConfirmationKas()
        {
            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(KeyAgreementMacType.AesCcm)
                .WithMacLength(0)
                .WithNonce(new BitString(1))
                .Build();

            var result = _subject
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(EccScheme.OnePassMqv, EccParameterSet.Eb, _curve))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildKdfNoKc()
                .WithKeyLength(0)
                .WithOtherInfoPattern(string.Empty)
                .WithMacParameters(macParams)
                .Build();

            Assert.AreEqual(KasMode.KdfNoKc, result.Scheme.SchemeParameters.KasMode);
        }

        [Test]
        public void ShouldReturnKeyConfirmationKas()
        {
            var macParams = _macParamsBuilder
                .WithKeyAgreementMacType(KeyAgreementMacType.AesCcm)
                .WithMacLength(0)
                .WithNonce(new BitString(1))
                .Build();

            var result = _subject
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesEcc(EccScheme.OnePassMqv, EccParameterSet.Eb, _curve))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildKdfKc()
                .WithKeyLength(0)
                .WithOtherInfoPattern(string.Empty)
                .WithMacParameters(macParams)
                .WithKeyConfirmationRole(KeyConfirmationRole.Provider)
                .WithKeyConfirmationDirection(KeyConfirmationDirection.Bilateral)
                .WithKeyLength(128)
                .Build();

            Assert.AreEqual(KasMode.KdfKc, result.Scheme.SchemeParameters.KasMode);
        }
    }
}