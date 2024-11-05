using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SpComponent;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SPComponent
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithCorrectSignature()
        {
            var subject = new TestCaseValidator(GetTestCase());
            var result = await subject.ValidateAsync(GetTestCase());

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithIncorrectSignature()
        {
            var responseTestCase = GetTestCase();
            responseTestCase.Signature = new BitString("1234");

            var subject = new TestCaseValidator(GetTestCase());
            var result = await subject.ValidateAsync(responseTestCase);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailWithMismatchedFailingTests()
        {
            var responseTestCase = GetFailureTestCase();
            responseTestCase.TestPassed = true;

            var subject = new TestCaseValidator(GetFailureTestCase());
            var result = await subject.ValidateAsync(responseTestCase);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndFailIfNoSignatureProvidedWhenExpected()
        {
            var responseTestCase = GetFailureTestCase();
            responseTestCase.TestPassed = true;

            var subject = new TestCaseValidator(GetTestCase());
            var result = await subject.ValidateAsync(responseTestCase);

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
        }

        [Test]
        public async Task ShouldRunVerifyMethodAndSucceedWithMatchingFailureTests()
        {
            var subject = new TestCaseValidator(GetFailureTestCase());
            var result = await subject.ValidateAsync(GetFailureTestCase());

            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed));
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                Signature = new BitString("ABCD"),
                TestPassed = true
            };
        }

        private TestCase GetFailureTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = false
            };
        }
    }
}
