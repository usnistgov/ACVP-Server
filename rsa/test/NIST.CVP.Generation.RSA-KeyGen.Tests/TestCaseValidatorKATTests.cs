using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorKATTests
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldValidateIfExpectedAndSuppliedResultsMatch(bool failureTest)
        {
            var testCase = GetTestCase(failureTest);
            var subject = new TestCaseValidatorKAT(testCase);

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldNotValidateIfExpectedAndSuppliedDoNotMatch(bool failureTest)
        {
            var testCase = GetTestCase(failureTest);
            var subject = new TestCaseValidatorKAT(GetTestCase(!failureTest));

            var result = subject.Validate(testCase);

            Assume.That(result != null);
            Assert.AreEqual("failed", result.Result);
        }

        private TestCase GetTestCase(bool failureTest)
        {
            return new TestCase
            {
                TestCaseId = 1,
                FailureTest = failureTest
            };
        }
    }
}
