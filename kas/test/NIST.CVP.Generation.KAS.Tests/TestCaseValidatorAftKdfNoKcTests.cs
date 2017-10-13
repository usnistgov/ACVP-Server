using System;
using System.Linq;
using Moq;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Builders;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.KAS.Scheme;
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
    public class TestCaseValidatorAftKdfNoKcTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private Mock<IKasBuilder> _kasBuilder;
        private Mock<IKasBuilderKdfNoKc> _kasBuilderKdfNoKc;
        private Mock<IKas> _kas;
        private Mock<IScheme> _scheme;
        private TestCaseValidatorAftKdfNoKc _subject;

        private ShaFactory _shaFactory;
        private IMacParametersBuilder _macParametersBuilder;
        private ISchemeBuilder _schemeBuilder;
        private IEntropyProviderFactory _entropyProviderFactory;

        [SetUp]
        public void Setup()
        {
            HashFunction hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);

            _entropyProviderFactory = new EntropyProviderFactory();
            _shaFactory = new ShaFactory();
            _macParametersBuilder = new MacParametersBuilder();
            _schemeBuilder = new SchemeBuilder(
                new FfcDsa(_shaFactory.GetShaInstance(hashFunction)),
                new KdfFactory(_shaFactory),
                new KeyConfirmationFactory(),
                new NoKeyConfirmationFactory(),
                new OtherInfoFactory(_entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random)),
                _entropyProviderFactory.GetEntropyProvider(EntropyProviderTypes.Random),
                new DiffieHellman(),
                new Mqv()
            );

            _scheme = new Mock<IScheme>();
            _scheme.Setup(s => s.EphemeralKeyPair)
                .Returns(new FfcKeyPair(1, 2));
            _scheme.Setup(s => s.StaticKeyPair)
                .Returns(new FfcKeyPair(1, 2));

            _kas = new Mock<IKas>();
            _kas.Setup(s => s.Scheme)
                .Returns(_scheme.Object);

            _kasBuilderKdfNoKc = new Mock<IKasBuilderKdfNoKc>();
            _kasBuilderKdfNoKc
                .Setup(s => s.WithKeyLength(It.IsAny<int>()))
                .Returns(_kasBuilderKdfNoKc.Object);
            _kasBuilderKdfNoKc
                .Setup(s => s.WithMacParameters(It.IsAny<MacParameters>()))
                .Returns(_kasBuilderKdfNoKc.Object);
            _kasBuilderKdfNoKc
                .Setup(s => s.WithOtherInfoPattern(It.IsAny<string>()))
                .Returns(_kasBuilderKdfNoKc.Object);
            _kasBuilderKdfNoKc.Setup(s => s.Build()).Returns(_kas.Object);

            _kasBuilder = new Mock<IKasBuilder>();
            _kasBuilder
                .Setup(s => s.WithKeyAgreementRole(It.IsAny<KeyAgreementRole>()))
                .Returns(_kasBuilder.Object);
            _kasBuilder
                .Setup(s => s.WithParameterSet(It.IsAny<FfcParameterSet>()))
                .Returns(_kasBuilder.Object);
            _kasBuilder
                .Setup(s => s.WithPartyId(It.IsAny<BitString>()))
                .Returns(_kasBuilder.Object);
            _kasBuilder
                .Setup(s => s.WithSchemeBuilder(It.IsAny<ISchemeBuilder>()))
                .Returns(_kasBuilder.Object);
            _kasBuilder
                .Setup(s => s.WithAssurances(It.IsAny<KasAssurance>()))
                .Returns(_kasBuilder.Object);
            _kasBuilder
                .Setup(s => s.WithScheme(It.IsAny<FfcScheme>()))
                .Returns(_kasBuilder.Object);
            _kasBuilder.Setup(s => s.BuildKdfNoKc()).Returns(_kasBuilderKdfNoKc.Object);
        }

        [Test]
        public void ShouldSucceedValidation()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            SetupKasMockForGroupAndCast(testGroup, testCase);

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _kasBuilder.Object, _macParametersBuilder,
                _schemeBuilder, _entropyProviderFactory);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("passed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        [TestCase(FfcScheme.DhEphem)]
        public void ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme)
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];
            testGroup.Scheme = FfcScheme.DhEphem;
            testCase.EphemeralPublicKeyIut = 0;

            SetupKasMockForGroupAndCast(testGroup, testCase);

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _kasBuilder.Object, _macParametersBuilder,
                _schemeBuilder, _entropyProviderFactory);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ShouldFailWhenIutDoesNotProvideDkm()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            SetupKasMockForGroupAndCast(testGroup, testCase);

            testCase.Dkm = null;

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _kasBuilder.Object, _macParametersBuilder,
                _schemeBuilder, _entropyProviderFactory);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }


        [Test]
        public void ShouldFailWhenIutDoesNotProvideTag()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            SetupKasMockForGroupAndCast(testGroup, testCase);

            testCase.Tag = null;

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _kasBuilder.Object, _macParametersBuilder,
                _schemeBuilder, _entropyProviderFactory);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ShouldFailWhenMismatchedDkm()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            SetupKasMockForGroupAndCast(testGroup, testCase);

            testCase.Dkm[0] += 2;

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _kasBuilder.Object, _macParametersBuilder,
                _schemeBuilder, _entropyProviderFactory);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ShouldFailWhenMismatchedTag()
        {
            var testGroup = GetData();
            var testCase = (TestCase)testGroup.Tests[0];

            SetupKasMockForGroupAndCast(testGroup, testCase);

            testCase.Tag[0] += 2;

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _kasBuilder.Object, _macParametersBuilder,
                _schemeBuilder, _entropyProviderFactory);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result.Equals("failed", StringComparison.OrdinalIgnoreCase));
        }

        private void SetupKasMockForGroupAndCast(TestGroup testGroup, TestCase testCase)
        {
            _kas
                .Setup(s => s.ReturnPublicInfoThisParty())
                .Returns(
                    new FfcSharedInformation(
                        new FfcDomainParameters(testGroup.P, testGroup.Q, testGroup.G),
                        testGroup.IdServer,
                        testCase.StaticPublicKeyServer,
                        testCase.EphemeralPublicKeyServer,
                        null,
                        null,
                        null
                    )
                );
            _kas
                .Setup(s => s.ComputeResult(It.IsAny<FfcSharedInformation>()))
                .Returns(new KasResult(testCase.Z.GetDeepCopy(), testCase.OtherInfo.GetDeepCopy(),
                    testCase.Dkm.GetDeepCopy(), testCase.MacData.GetDeepCopy(), testCase.Tag.GetDeepCopy()));
        }

        private TestGroup GetData()
        {
            var testGroup = _tdm.GetTestGroups().First();
            testGroup.KasMode = KasMode.KdfNoKc;

            return testGroup;
        }
    }
}