using System.Linq;
using Moq;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftNoKdfNoKcTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private TestCaseValidatorAftNoKdfNoKc _subject;
        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>>();
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
        public void ShouldSucceedValidation(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);
            
            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(new KasResult(testCase.Z, testCase.HashZ));

            var result = _subject.Validate(testCase);

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
        public void ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];
            testCase.EphemeralPublicKeyIutX = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

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
        public void ShouldFailWhenIutDoesNotProvideStaticKeyPair(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];
            testCase.StaticPublicKeyIutX = 0;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

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
        public void ShouldFailWhenIutDoesNotProvideHashZ(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];
            
            testCase.HashZ = null;

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

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
        public void ShouldFailWhenMismatchedHashZ(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup = GetData(scheme, kasRole);
            var testCase = (TestCase)testGroup.Tests[0];

            BitString newValue = testCase.HashZ.GetDeepCopy();
            newValue[0] += 2;

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(new KasResult(testCase.Z, newValue));

            _subject = new TestCaseValidatorAftNoKdfNoKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }
        
        private TestGroup GetData(EccScheme scheme, KeyAgreementRole kasRole)
        {
            var testGroup =  _tdm.GetTestGroups().First();
            testGroup.KasMode = KasMode.NoKdfNoKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;

            return testGroup;
        }
    }
}