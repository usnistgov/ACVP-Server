using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KES;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.KAS.Enums;
using NIST.CVP.Generation.KAS.FFC;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorValNoKdfNoKcTests
    {
        // Note using sha2-256 in tests
        private readonly HashFunction _hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
        private readonly FfcDomainParameters _domainParameters = new FfcDomainParameters(
            new BitString("d5cf9ba288ff8438650904a5fe2eeb8bf6b52b691a455b21bd3b37998f82544036ae61fe436039c66feab83ba21a5ba13e7b4c1692ac82c65309eed54e593efe9831cc82bfd11e9552d4eb7d8f2c233024bc10819c57093890ec19aa9ee915e524d81c285928a5b87acc7f496f93689ed59b15183689ec5e487fdc9fb994c4bf7e1dba57f8f12c17e404f68b5506d69f4aa98146f6ba3be1f3397e5d41c4235a18b38c75417bb9091a487db1fc89eb2f73b4ac6f6c12dc5634a342cd9d10605090bd544e1c0813aaaa3814166df84a302b8df38babe9c31e4a2c64317046bdb841fa8717e22dcf1496f9ad94c17f1ff9175567d02ab5e30601e2e5b0884e9a77").ToPositiveBigInteger(),
            new BitString("ceb9916bbc14cdc9dda80481135bee68ee94f4ecadc2921261a316d1c9cf9283").ToPositiveBigInteger(),
            new BitString("37a7b2a094ed6253784d51c71a4a7407f0727df15e480a02b0ff2f9befdb0e92c0d63e482c6909e5337373abe347ceaec25a9f2a23f1770c2447bfb5c35dbda80d3f00d8b6569a0165a3d52f1cff03e02b67be47b2f26ebd0fc13299fd0317719c3bcbbcbba0a982915b5d68fb4c5c483c3df12052b56ceacd16176c783d56422b1366cd5ce65922b734a1780acf35b4a658cfe8166469ace04b87a33e1e8d4603ae3f9607d1e708137d581aaf2cd86608ca06e52cddbe475f8bdb4597cd5b55f47380276c86d08d8ddbbef4ee3ca76cefe6ff9c74d4d5d1be3407e30720be34cf41f482be8bc6ed6e89ea7cc2d164da1dc1c9d0e2762eef983dc9e96f3bfd50").ToPositiveBigInteger()
        );
        private TestCaseGeneratorValNoKdfNoKc _subject;
        private FfcDsa _dsa;
        private IEntropyProvider _entropyProvider;
        private IKasBuilder _kasBuilder;
        private ISchemeBuilder _schemeBuilder;
        private Mock<IDsaFfcFactory> _dsaFactory;
        private IShaFactory _shaFactory;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new ShaFactory();
            _dsa = new FfcDsa(_shaFactory.GetShaInstance(_hashFunction));

            _dsaFactory = new Mock<IDsaFfcFactory>();
            _dsaFactory
                .Setup(s => s.GetInstance(It.IsAny<HashFunction>(), It.IsAny<EntropyProviderTypes>()))
                .Returns(_dsa);

            _entropyProvider = new EntropyProvider(new Random800_90());
            _schemeBuilder = new SchemeBuilder(
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

            _subject = new TestCaseGeneratorValNoKdfNoKc(
                _kasBuilder, _schemeBuilder, _dsaFactory.Object, TestCaseDispositionOption.Success
            );
        }

        [Test]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]
        public void ShouldPopulateCorrectKeysNoncesForSchemeRole(FfcScheme scheme, KeyAgreementRole testGroupIutRole)
        {
            BuildTestGroup(scheme, testGroupIutRole, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

            Assert.IsFalse(resultTestCase.Deferred, nameof(resultTestCase.Deferred));
            #region KeyCheck
            if (serverKeyGenRequirements.GeneratesStaticKeyPair)
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

            if (serverKeyGenRequirements.GeneratesEphemeralKeyPair)
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

            if (iutKeyGenRequirements.GeneratesStaticKeyPair)
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyIut != 0,
                    nameof(resultTestCase.StaticPrivateKeyIut));
                Assert.IsTrue(resultTestCase.StaticPublicKeyIut != 0,
                    nameof(resultTestCase.StaticPublicKeyIut));
            }
            else
            {
                Assert.IsTrue(resultTestCase.StaticPrivateKeyIut == 0,
                    nameof(resultTestCase.StaticPrivateKeyIut));
                Assert.IsTrue(resultTestCase.StaticPublicKeyIut == 0,
                    nameof(resultTestCase.StaticPublicKeyIut));
            }

            if (iutKeyGenRequirements.GeneratesEphemeralKeyPair)
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut != 0,
                    nameof(resultTestCase.EphemeralPrivateKeyIut));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyIut != 0,
                    nameof(resultTestCase.EphemeralPublicKeyIut));
            }
            else
            {
                Assert.IsTrue(resultTestCase.EphemeralPrivateKeyIut == 0,
                    nameof(resultTestCase.EphemeralPrivateKeyIut));
                Assert.IsTrue(resultTestCase.EphemeralPublicKeyIut == 0,
                    nameof(resultTestCase.EphemeralPublicKeyIut));
            }
            #endregion KeyCheck
        }

        [Test]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedOi)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedOi)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedDkm)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedDkm)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedMacData)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedMacData)]
        public void ShouldSetInvalidTestCaseDispositionsToSuccess(FfcScheme scheme, KeyAgreementRole testGroupIutRole, TestCaseDispositionOption option)
        {
            _subject = new TestCaseGeneratorValNoKdfNoKc(_kasBuilder, _schemeBuilder, _dsaFactory.Object,  option);

            BuildTestGroup(scheme, testGroupIutRole, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

            Assert.AreEqual(TestCaseDispositionOption.Success, resultTestCase.TestCaseDisposition);
        }

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.Success, false)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.Success, false)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailAssuranceServerEphemeralPublicKey, true)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedTag, true)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, TestCaseDispositionOption.FailChangedZ, true)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, TestCaseDispositionOption.FailChangedZ, true)]
        public void ShouldSetProperTestCaseFailureTestProperty(FfcScheme scheme, KeyAgreementRole testGroupIutRole,
            TestCaseDispositionOption option, bool isFailure)
        {
            _subject = new TestCaseGeneratorValNoKdfNoKc(_kasBuilder, _schemeBuilder, _dsaFactory.Object, option);

            BuildTestGroup(scheme, testGroupIutRole, out var iutKeyGenRequirements, out var serverKeyGenRequirements, out var resultTestCase);

            Assert.AreEqual(isFailure, resultTestCase.FailureTest);
        }

        private void BuildTestGroup(
            FfcScheme scheme, 
            KeyAgreementRole testGroupIutRole, 
            out SchemeKeyNonceGenRequirement iutKeyGenRequirements, 
            out SchemeKeyNonceGenRequirement serverKeyGenRequirements, 
            out TestCase resultTestCase
        )
        {
            TestGroup tg = new TestGroup()
            {
                KasMode = KasMode.NoKdfNoKc,
                Scheme = scheme,
                KasRole = testGroupIutRole,
                Function = KasAssurance.None,
                HashAlg = _hashFunction,
                KcRole = KeyConfirmationRole.None,
                KcType = KeyConfirmationDirection.None,
                MacType = KeyAgreementMacType.None,
                TestType = "VAL",
                P = _domainParameters.P,
                Q = _domainParameters.Q,
                G = _domainParameters.G
            };

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