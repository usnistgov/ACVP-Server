using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ecc;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.FixedInfo;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.Builders
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
                    new KdfOneStepFactory(
                        new NativeShaFactory(), new HmacFactory(new NativeShaFactory()), new KmacFactory(new CSHAKEWrapper())
                    ),
                    new KeyConfirmationFactory(new KeyConfirmationMacDataCreator()),
                    new NoKeyConfirmationFactory(new NoKeyConfirmationMacDataCreator()),
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
