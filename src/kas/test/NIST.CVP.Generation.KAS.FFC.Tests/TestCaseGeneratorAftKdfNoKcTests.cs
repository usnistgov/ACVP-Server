using Moq;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Builders;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Schema;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme.Ffc;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAftKdfNoKcTests
    {
        private TestCaseGeneratorAftKdfNoKc _subject;
        private Mock<IDsaFfc> _dsa;
        private IEntropyProvider _entropyProvider;
        private IEntropyProviderFactory _entropyProviderFactory;
        private IKasBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _kasBuilder;
        private ISchemeBuilder<KasDsaAlgoAttributesFfc, OtherPartySharedInformation<FfcDomainParameters, FfcKeyPair>, FfcDomainParameters, FfcKeyPair> _schemeBuilder;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private IMacParametersBuilder _macParametersBuilder;

        [SetUp]
        public void Setup()
        {
            _dsa = new Mock<IDsaFfc>();
            _dsa
                .Setup(s => s.GenerateDomainParameters(It.IsAny<FfcDomainParametersGenerateRequest>()))
                .Returns(
                    new FfcDomainParametersGenerateResult(
                        new FfcDomainParameters(1, 2, 3),
                        new DomainSeed(1),
                        new Counter(1)
                    )
                );
            _dsa
                .Setup(s => s.GenerateKeyPair(It.IsAny<FfcDomainParameters>()))
                .Returns(
                    new FfcKeyPairGenerateResult(
                        new FfcKeyPair(4, 5)
                    )
                );
            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa.Object);

            _entropyProvider = new EntropyProvider(new Random800_90());
            _entropyProviderFactory = new EntropyProviderFactory();
            _schemeBuilder = new SchemeBuilderFfc(
                    _dsaFactory.Object,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(_entropyProvider),
                    _entropyProvider,
                    new DiffieHellmanFfc(),
                    new MqvFfc()
            );
            _kasBuilder = new KasBuilderFfc(_schemeBuilder);
            
            _macParametersBuilder = new MacParametersBuilder();

            _subject = new TestCaseGeneratorAftKdfNoKc(
                _kasBuilder, _schemeBuilder, _entropyProviderFactory, _macParametersBuilder
            );
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.AesCcm)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.CmacAes)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D224)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D256)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D384)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyAgreementMacType.HmacSha2D512)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyAgreementMacType.HmacSha2D512)]

        public void ShouldPopulateCorrectKeysNoncesForSchemeRole(FfcScheme scheme, KeyAgreementRole testGroupIutRole, KeyAgreementMacType macType)
        {
            TestGroup tg = new TestGroup()
            {
                KasMode = KasMode.KdfNoKc,
                Scheme = scheme,
                KasRole = testGroupIutRole,
                Function = KasAssurance.None,
                HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                KcRole = KeyConfirmationRole.None,
                KcType = KeyConfirmationDirection.None,
                MacType = macType,
                KeyLen = 128,
                MacLen = 128,
                KdfType = "concatenation",
                OiPattern = "uPartyInfo||vPartyInfo",
                TestType = "AFT",
                P = new BitString("99fa9940cbff808725a37913f36464f06975982a78bd37a3c25a3fd0d1f3b96422e38456ed184b04eb6596060bba808de0de809835ae826aac778c1b142e641034d55c91082b2da9a0e2bbe3a41cf306b1fa8612516831eefc0f7fcdeed4eac8e108b8a8c3512153c03f5b4c7da618bf2724a8c12a798550ac1758bcac93e7ab6e62f5bf7f1ab777505c9d80b889dfeadc0f60c6359d356a94a991990f028f4a73b34b1a4a18c8a77d447f4570a1d558ebcd6432d191065ae7791e33c3ad3401e45cdd48bf2d2195b0cfc3252d1ae2418506e890ebf12c53f9e9a61631beca30fbe1b29e99511376e540d74c956dd1849b7416d484861b8dedcff81d78c3657b").ToPositiveBigInteger(),
                Q = new BitString("e591e0ede4a3a50f98f093019d87fde2d910a335bdb7b975afaba453").ToPositiveBigInteger(),
                G = new BitString("250608957dc0a207723e1ed9d0961ef59a62aeb46b6d8b867f20f440c72161f3b485319ab0972b59ffa4c581531d725c9e4ae44159bae9eb795cc7c9589d48b5d439f73a6af08bcf931b36b5c7e44fcb39b4c3e6f9375ec5514f969f32f85666e165b74d4394669e71a32257260495b2c952a7c0c6c5708e0b2c0d852c9baae4aa9412aeabf27708123e2eddc2c4cc1b93c6956250a412767fd5bef398a914ca055443fb7fe9f0a3697c62fd8a64f345d54791af44d7c73599b4fdb92fdc44b4696770e3feb6ced406fd566ad1e334d5feaa76c92a02e0d784985c9ef154bf7d45d5c74fd995050d6e8acb20d4ae0d37d4684175731de1a3700db6fd297fe7b3").ToPositiveBigInteger()
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
                Assert.IsTrue(resultTestCase.StaticPublicKeyServer != 0,
                    nameof(resultTestCase.StaticPublicKeyServer));
            }
            else
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyServer == 0,
                    nameof(resultTestCase.StaticPrivateKeyServer));
                Assert.IsTrue(resultTestCase.StaticPublicKeyServer == 0,
                    nameof(resultTestCase.StaticPublicKeyServer));
            }

            if (keyGenRequirements.GeneratesEphemeralKeyPair)
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer != 0,
                    nameof(resultTestCase.EphemeralPrivateKeyServer));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServer != 0,
                    nameof(resultTestCase.EphemeralPublicKeyServer));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyServer == 0,
                    nameof(resultTestCase.EphemeralPrivateKeyServer));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyServer == 0,
                    nameof(resultTestCase.EphemeralPublicKeyServer));
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
        }
    }
}