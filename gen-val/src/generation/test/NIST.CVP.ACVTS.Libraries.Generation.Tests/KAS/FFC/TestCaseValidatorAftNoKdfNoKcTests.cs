using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.KAS.v1_0.FFC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KAS.FFC
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftNoKdfNoKcTests
    {
        private TestCaseValidatorAftNoKdfNoKc _subject;
        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>>();
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

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            _deferredResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(testGroup, testCase, testCase))
                .Returns(Task.FromResult(new KasResult(testCase.Z, testCase.HashZ)));

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Passed, Is.True);
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

        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];
            testCase.EphemeralKeyIut.PublicKeyY = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenIutDoesNotProvideStaticKeyPair(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];
            testCase.StaticKeyIut.PublicKeyY = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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
        public async Task ShouldFailWhenIutDoesNotProvideHashZ(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            testCase.HashZ = null;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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
        public async Task ShouldFailWhenMismatchedHashZ(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            BitString newValue = testCase.HashZ.GetDeepCopy();
            newValue[0] += 2;

            _deferredResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(testGroup, testCase, testCase))
                .Returns(Task.FromResult(new KasResult(testCase.Z, newValue)));

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
        }

        private TestGroup GetData(FfcScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = TestDataMother.GetTestGroups(1, true, "aft").TestGroups[0];
            testGroup.KasMode = KasMode.NoKdfNoKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;

            return testGroup;
        }
    }
}
