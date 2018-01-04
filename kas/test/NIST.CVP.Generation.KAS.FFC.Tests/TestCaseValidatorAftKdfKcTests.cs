using System.Linq;
using Moq;
using NIST.CVP.Crypto.KAS;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KAS.FFC.Tests
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
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        // Note dhEphem does not allow for key confirmation
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldSucceedValidation(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
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
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        // DhEphem does not allow for key confirmation
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        public void ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.EphemeralPublicKeyIut = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        // DhEphem does not allow for key confirmation
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        public void ShouldFailWhenIutDoesNotProvideStaticKeyPair(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.StaticPublicKeyIut = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideDkmNonce(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.DkmNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideEphemeralNonce(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.EphemeralNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenIutDoesNotProvideTag(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = (TestCase)testGroup.Tests[0];

            testCase.Tag = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = _subject.Validate(testCase);

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        [Test]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybrid1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv2, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhHybridOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.Mqv1, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        //[TestCase(FfcScheme.DhOneFlow, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]

        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral)]
        [TestCase(FfcScheme.DhStatic, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral)]
        public void ShouldFailWhenMismatchedTag(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
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

        private TestGroup GetData(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = _tdm.GetTestGroups().First();
            testGroup.KasMode = KasMode.KdfKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;
            testGroup.KcRole = kcRole;
            testGroup.KcType = kcType;

            ((TestCase) testGroup.Tests[0]).EphemeralNonceIut = new BitString("01");
            ((TestCase)testGroup.Tests[0]).EphemeralNonceServer = new BitString("02");

            return testGroup;
        }
    }
}