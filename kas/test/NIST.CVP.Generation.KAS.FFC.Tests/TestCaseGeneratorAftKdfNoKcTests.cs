using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Builders.Ffc;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
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
                TestType = "AFT"
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
        }
    }
}