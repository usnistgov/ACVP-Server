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
    public class TestCaseValidatorAftKdfKcTests
    {
        private TestCaseValidatorAftKdfKc _subject;

        private Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>> _deferredResolver;

        [SetUp]
        public void Setup()
        {
            _deferredResolver = new Mock<IDeferredTestCaseResolverAsync<TestGroup, TestCase, KasResult>>();
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
        public async Task ShouldSucceedValidation(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

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

            Assert.That(result.Result == Core.Enums.Disposition.Passed, Is.True);
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

        public async Task ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.EphemeralKeyIut.PublicKeyY = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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

        public async Task ShouldFailWhenIutDoesNotProvideStaticKeyPair(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.StaticKeyIut.PublicKeyY = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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
        public async Task ShouldFailWhenIutDoesNotProvideDkmNonce(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.DkmNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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
        public async Task ShouldFailWhenIutDoesNotProvideEphemeralNonce(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.EphemeralNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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
        public async Task ShouldFailWhenIutDoesNotProvideTag(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.Tag = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
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
        public async Task ShouldFailWhenMismatchedTag(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
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

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

            Assert.That(result.Result == Core.Enums.Disposition.Failed, Is.True);
        }

        private TestGroup GetData(FfcScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = TestDataMother.GetTestGroups(1, true, "aft").TestGroups[0];
            testGroup.KasMode = KasMode.KdfKc;
            testGroup.Scheme = scheme;
            testGroup.KasRole = kasRole;
            testGroup.KcRole = kcRole;
            testGroup.KcType = kcType;

            testGroup.Tests[0].EphemeralNonceIut = new BitString("01");
            testGroup.Tests[0].EphemeralNonceServer = new BitString("02");

            return testGroup;
        }
    }
}
