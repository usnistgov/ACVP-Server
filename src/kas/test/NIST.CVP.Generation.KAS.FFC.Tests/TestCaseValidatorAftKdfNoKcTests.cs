using Moq;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.KAS.v1_0.FFC;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.KAS.FFC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftKdfNoKcTests
    {
        private TestCaseValidatorAftKdfNoKc _subject;

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>>();
            _deferredResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(It.IsAny<TestGroup>(), It.IsAny<TestCase>(),
                    It.IsAny<TestCase>()))
                .Returns(() => Task.FromResult(new KasResult(0.ToBitString(), 1.ToBitString())));
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]

        public async Task ShouldSucceedValidation(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            _deferredResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(testGroup, testCase, testCase))
                .Returns(
                    Task.FromResult(new KasResult(
                        testCase.Z,
                        testCase.OtherInfo,
                        testCase.Dkm,
                        testCase.MacData,
                        testCase.Tag
                    ))
                );

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Passed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        public async Task ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            testCase.EphemeralPublicKeyIut = 0.ToBitString();

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]

        public async Task ShouldFailWhenIutDoesNotProvideStaticKeyPair(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            testCase.StaticPublicKeyIut = 0.ToBitString();

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]

        public async Task ShouldFailWhenIutDoesNotProvideDkmNonce(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            testCase.DkmNonceIut = null;

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenIutDoesNotProvideTag(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            testCase.Tag = null;

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenMismatchedTag(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            BitString newValue = testCase.Tag.GetDeepCopy();
            newValue[0] += 2;

            _deferredResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(testGroup, testCase, testCase))
                .Returns(
                    Task.FromResult(new KasResult(
                        testCase.Z,
                        testCase.OtherInfo,
                        testCase.Dkm,
                        testCase.MacData,
                        newValue
                    ))
                );

            _subject = new TestCaseValidatorAftKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        private TestGroup GetData(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = TestDataMother.GetTestGroups(1, true, "aft").TestGroups[0];
            testGroup.KasMode = KasMode.KdfNoKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;

            return testGroup;
        }
    }
}