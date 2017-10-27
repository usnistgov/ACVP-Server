using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorAftNoKdfNoKcTests
    {
        private TestCaseGeneratorAftNoKdfNoKc _subject;
        private Mock<IDsaFfc> _dsa;
        private IEntropyProvider _entropyProvider;
        private IKasBuilder _kasBuilder;
        private ISchemeBuilder _schemeBuilder;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private IShaFactory _shaFactory;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new ShaFactory();
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
            _schemeBuilder = new SchemeBuilder(
                    _shaFactory,
                    _dsaFactory.Object,
                    new KdfFactory(
                        new ShaFactory()
                    ),
                    new KeyConfirmationFactory(),
                    new NoKeyConfirmationFactory(),
                    new OtherInfoFactory(
                        _entropyProvider
                    ),
                    _entropyProvider,
                    new DiffieHellman(),
                    new Mqv()
            );
            _kasBuilder = new KasBuilder(_schemeBuilder);


            _subject = new TestCaseGeneratorAftNoKdfNoKc(
                _kasBuilder, _schemeBuilder, _dsaFactory.Object, _shaFactory
            );
        }

        [Test]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]
        public void ShouldPopulateCorrectKeysNoncesForSchemeRole(FfcScheme scheme, KeyAgreementRole testGroupIutRole)
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
                MacType = KeyAgreementMacType.None,
                TestType = "AFT"
            };

            KeyAgreementRole serverRole = KeyAgreementRole.InitiatorPartyU;
            if (testGroupIutRole == KeyAgreementRole.InitiatorPartyU)
            {
                serverRole = KeyAgreementRole.ResponderPartyV;
            }

            var keyGenRequirements = SpecificationMapping.GetKeyGenerationOptionsForSchemeAndRole(scheme, serverRole);

            var result = _subject.Generate(tg, false);
            var resultTestCase = (TestCase) result.TestCase;

            Assert.IsTrue(resultTestCase.Deferred, nameof(resultTestCase.Deferred));
            if (keyGenRequirements.generatesStaticKeyPair)
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

            if (keyGenRequirements.generatesEphemeralKeyPair)
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