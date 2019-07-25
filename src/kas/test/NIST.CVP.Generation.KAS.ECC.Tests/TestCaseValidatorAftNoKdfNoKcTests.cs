using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.KAS.v1_0.ECC;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
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
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldSucceedValidation(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);
            
            _deferredResolver
                .Setup(s => s.CompleteDeferredCryptoAsync(testGroup, testCase, testCase))
                .Returns(Task.FromResult(new KasResult(testCase.Z, testCase.HashZ)));

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Passed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];
            testCase.EphemeralKeyIut.PublicQ.X = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenIutDoesNotProvideStaticKeyPair(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];
            testCase.StaticKeyIut.PublicQ.X = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenIutDoesNotProvideHashZ(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = testGroup.Tests[0];
            
            testCase.HashZ = null;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV)]
        public async Task ShouldFailWhenMismatchedHashZ(EccScheme scheme, KeyAgreementRole kasRole)
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

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }
        
        private TestGroup GetData(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = TestDataMother.GetTestGroups(1, false, "aft").TestGroups[0];
            testGroup.KasMode = KasMode.NoKdfNoKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;

            return testGroup;
        }
    }
}