using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SSH
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
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
        public async Task ShouldFailIfValueDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.IntegrityKeyClient = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(result.Result, Is.EqualTo(Core.Enums.Disposition.Failed));
            Assert.That(result.Reason.Contains(nameof(suppliedResult.IntegrityKeyClient)), Is.True);
        }

        [Test]
        public async Task ShouldShowValueAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.EncryptionKeyServer = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.That(result.Reason.Contains(nameof(suppliedResult.EncryptionKeyServer)), Is.True);
        }

        [Test]
        public async Task ShouldFailIfValueNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.InitialIvClient = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assert.That(result != null);
            Assert.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.That(result.Reason.Contains($"{nameof(suppliedResult.InitialIvClient)} was not present in the {nameof(TestCase)}"), Is.True);
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                InitialIvClient = new BitString("01ABCDEF0123456789ABCDEF012345678901"),
                EncryptionKeyClient = new BitString("02ABCDEF0123456789ABCDEF012345678901"),
                IntegrityKeyClient = new BitString("03ABCDEF0123456789ABCDEF012345678901"),

                InitialIvServer = new BitString("04ABCDEF0123456789ABCDEF012345678901"),
                EncryptionKeyServer = new BitString("05ABCDEF0123456789ABCDEF012345678901"),
                IntegrityKeyServer = new BitString("06ABCDEF0123456789ABCDEF012345678901"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
