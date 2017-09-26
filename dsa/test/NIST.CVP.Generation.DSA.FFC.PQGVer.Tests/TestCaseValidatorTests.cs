using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        public void ShouldRunVerifyMethodAndSucceedWithGoodTest()
        {
            var subject = new TestCaseValidator(GetTestCase());
            var result = subject.Validate(GetResultTestCase(true));

            Assert.AreEqual("passed", result.Result);
        }

        [Test]
        public void ShouldRunVerifyMethodAndFailWithBadTest()
        {
            var subject = new TestCaseValidator(GetTestCase());
            var result = subject.Validate(GetResultTestCase(false));

            Assert.AreEqual("failed", result.Result);
        }

        private TestCase GetTestCase()
        {
            return new TestCase
            {
                TestCaseId = 1,
                FailureTest = true  // Says the expected result is "failed"
            };
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                P = 2,
                Q = 3,
                Result = shouldPass   // Says the test "passed"
            };
        }
    }
}
