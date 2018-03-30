using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.IKEv2.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result, result.Reason);
        }

        [Test]
        public void ShouldFailIfValueDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.DerivedKeyingMaterialChild = new BitString("D00000");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
            Assert.IsTrue(result.Reason.Contains(nameof(suppliedResult.DerivedKeyingMaterialChild)));
        }

        [Test]
        public void ShouldShowValueAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.DerivedKeyingMaterial = new BitString("D00000");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assume.That(Core.Enums.Disposition.Failed == result.Result);
            Assert.IsTrue(result.Reason.Contains(nameof(suppliedResult.DerivedKeyingMaterial)));
        }

        [Test]
        public void ShouldFailIfValueNotPresent()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidator(testCase);
            var suppliedResult = GetTestCase();

            suppliedResult.SKeySeed = null;

            var result = subject.Validate(suppliedResult);
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
