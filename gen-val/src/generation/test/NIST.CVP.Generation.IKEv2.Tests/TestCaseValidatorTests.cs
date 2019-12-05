using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv2;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv2.Tests
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

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public async Task ShouldFailIfValueDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.DerivedKeyingMaterialChild = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains(nameof(suppliedResult.DerivedKeyingMaterialChild)));
        }

        [Test]
        public async Task ShouldShowValueAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.DerivedKeyingMaterial = new BitString("D00000");

            var result = await subject.ValidateAsync(suppliedResult);

            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains(nameof(suppliedResult.DerivedKeyingMaterial)));
        }

        [Test]
        public async Task ShouldFailIfValueNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.SKeySeed = null;

            var result = await subject.ValidateAsync(suppliedResult);
            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);

            Assert.IsTrue(result.Reason.Contains($"{nameof(suppliedResult.SKeySeed)} was not present in the {nameof(TestCase)}"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                SKeySeed = new BitString("01ABCDEF0123456789ABCDEF0123456789"),
                SKeySeedReKey = new BitString("02ABCDEF0123456789ABCDEF0123456789"),
                DerivedKeyingMaterial = new BitString("03ABCDEF0123456789ABCDEF0123456789"),
                DerivedKeyingMaterialChild = new BitString("04ABCDEF0123456789ABCDEF0123456789"),
                DerivedKeyingMaterialDh = new BitString("05ABCDEF0123456789ABCDEF0123456789"),
                TestCaseId = 1
            };
            return testCase;
        }
    }
}
