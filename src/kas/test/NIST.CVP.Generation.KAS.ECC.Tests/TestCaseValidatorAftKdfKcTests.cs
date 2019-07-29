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
        public async Task ShouldSucceedValidation(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
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
        public async Task ShouldFailWhenIutDoesNotProvideEphemeralKeyPair(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.EphemeralKeyIut.PublicQ.X = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

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
        public async Task ShouldFailWhenIutDoesNotProvideStaticKeyPair(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.StaticKeyIut.PublicQ.X = 0;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

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
        public async Task ShouldFailWhenIutDoesNotProvideDkmNonce(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.DkmNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

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
        public async Task ShouldFailWhenIutDoesNotProvideEphemeralNonce(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.EphemeralNonceIut = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

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
        public async Task ShouldFailWhenIutDoesNotProvideTag(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = GetData(scheme, kasRole, kcRole, kcType);
            var testCase = testGroup.Tests[0];

            testCase.Tag = null;

            _subject = new TestCaseValidatorAftKdfKc(testCase, testGroup, _deferredResolver.Object);

            var result = await _subject.ValidateAsync(testCase);

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
        public async Task ShouldFailWhenMismatchedTag(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
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

            Assert.IsTrue(result.Result == Core.Enums.Disposition.Failed);
        }

        private TestGroup GetData(EccScheme scheme, KeyAgreementRole kasRole, KeyConfirmationRole kcRole, KeyConfirmationDirection kcType)
        {
            var testGroup = TestDataMother.GetTestGroups(1, false, "aft").TestGroups[0];
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