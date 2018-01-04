using System.Linq;
using Moq;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.ECC.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorAftKdfKcTests
    {
        private readonly TestDataMother _tdm = new TestDataMother();
        private TestCaseValidatorAftKdfKc _subject;

        private Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolver<TestGroup, TestCase, KasResult>>();
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        // Note EphemeralUnified does not allow for key confirmation
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldSucceedValidation(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(
                    new KasResult(
                        testCase.Z,
                        testCase.OtherInfo,
                        testCase.Dkm,
                        testCase.MacData,
                        testCase.Tag
                    )
                );

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Passed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        // EphemeralUnified does not allow for key confirmation
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.EphemeralPublicKeyIutX = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        // EphemeralUnified does not allow for key confirmation
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideStaticKeyPair(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.StaticPublicKeyIutX = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideDkmNonce(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.DkmNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideEphemeralNonce(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.EphemeralNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideTag(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.Tag = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.FullMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.EphemeralUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.OnePassMqv, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(EccScheme.OnePassDh, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(EccScheme.StaticUnified, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenMismatchedTag(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            BitString newValue = testCase.Tag.GetDeepCopy();
            newValue[0] += 2;

            _deferredResolver
                .Setup(s => s.CompleteDeferredCrypto(testGroup, testCase, testCase))
                .Returns(
                    new KasResult(
                        testCase.Z,
                        testCase.OtherInfo,
                        testCase.Dkm,
                        testCase.MacData,
                        newValue
                    )
                );

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        private TestGroup GetData(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = _tdm.GetTestGroups().First();
            testGroup.KasMode = KasMode.KdfKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;
            testGroup.KcRole = kcRole;
            testGroup.KcType = kcType;

            ((TestCase)testGroup.Tests[0]).EphemeralNonceIut = new BitString("01");
            ((TestCase)testGroup.Tests[0]).EphemeralNonceServer = new BitString("02");

            return testGroup;
        }
    }
}