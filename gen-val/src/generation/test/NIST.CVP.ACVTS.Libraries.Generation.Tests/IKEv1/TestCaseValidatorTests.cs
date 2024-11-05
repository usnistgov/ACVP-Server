using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.IKEv1;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.IKEv1
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorEncryptTests
    {
        [Test]
        public async Task ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);

            var result = await subject.ValidateAsync(testCase);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Passed), result.Reason);
        }

        [Test]
        public async Task ShouldFailIfKeyOutDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.SKeyIdD = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains(nameof(suppliedResult.SKeyIdD)), Is.True);
        }

        [Test]
        public async Task ShouldShowKeyOutAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.SKeyIdA = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains(nameof(suppliedResult.SKeyIdA)), Is.True);
        }

        [Test]
        public async Task ShouldFailIfKeyOutNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.SKeyId = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.SKeyId)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                SKeyId = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                SKeyIdA = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                SKeyIdD = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                SKeyIdE = new BitString("ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
