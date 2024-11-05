using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Scheme;
using NIST.CVP.ACVTS.Libraries.Crypto.cSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.Builders.Ffc;
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
    public class KasBuilderFfcTests
    {

        private KasBuilderFfc _subject;
        private MacParametersBuilder _macParamsBuilder;
        private Mock<IDsaFfc> _dsa;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private IEntropyProvider _entropyProviderScheme;
        private IEntropyProvider _entropyProviderOtherInfo;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaFfc>();
            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);
            _entropyProviderScheme = new TestableEntropyProvider();
            _entropyProviderOtherInfo = new TestableEntropyProvider();

            _subject = new KasBuilderFfc(
                new SchemeBuilderFfc(
                    _dsaFactory.Object,
                    new KdfOneStepFactory(
                        new NativeShaFactory(), new HmacFactory(new NativeShaFactory()), new KmacFactory(new cSHAKEWrapper())
                    ),
                    new KeyConfirmationFactory(new KeyConfirmationMacDataCreator()),
                    new NoKeyConfirmationFactory(new NoKeyConfirmationMacDataCreator()),
                    new OtherInfoFactory(_entropyProviderOtherInfo),
                    _entropyProviderScheme,
                    new DiffieHellmanFfc(),
                    new MqvFfc()
                )
            );

            _macParamsBuilder = new MacParametersBuilder();
        }

        [Test]
        public void ShouldReturnComponentOnlyKas()
        {
            var result = _subject
                .WithKeyAgreementRole(KeyAgreementRole.InitiatorPartyU)
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildNoKdfNoKc()
                .Build();


            Assert.That(result.Scheme.SchemeParameters.KasMode, Is.EqualTo(KasMode.NoKdfNoKc));
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
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.DhEphem, FfcParameterSet.Fb))
                .WithAssurances(KasAssurance.None)
                .WithPartyId(new BitString(1))
                .BuildKdfNoKc()
                .WithKeyLength(0)
                .WithOtherInfoPattern(string.Empty)
                .WithMacParameters(macParams)
                .Build();

            Assert.That(result.Scheme.SchemeParameters.KasMode, Is.EqualTo(KasMode.KdfNoKc));
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
                .WithKasDsaAlgoAttributes(new KasDsaAlgoAttributesFfc(FfcScheme.Mqv1, FfcParameterSet.Fb))
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

            Assert.That(result.Scheme.SchemeParameters.KasMode, Is.EqualTo(KasMode.KdfKc));
        }
    }
}
