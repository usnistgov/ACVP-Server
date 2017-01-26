using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1.Tests
{
    [TestFixture]
    public class TestCaseValidatorHashTests
    {
        [Test]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldFailIfDigestDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("FACEDADE");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        [Test]
        public void ShouldShowDigestAsReasonIfItDoesNotMatch()
        {
            var testCase = GetTestCase();
            var subject = new TestCaseValidatorHash(testCase);
            var suppliedResult = GetTestCase();
            suppliedResult.Digest = new BitString("FACEDADE");

            var result = subject.Validate(suppliedResult);

            Assume.That(result != null);
            Assume.That("failed" == result.Result);
            Assert.IsTrue(result.Reason.Contains("Digest"));
        }

        private TestCase GetTestCase()
        {
            var testCase = new TestCase
            {
                Digest = new BitString("1234567890123456789012345678901234567890"),
                TestCaseId = 1
            };

            return testCase;
        }
    }
}
