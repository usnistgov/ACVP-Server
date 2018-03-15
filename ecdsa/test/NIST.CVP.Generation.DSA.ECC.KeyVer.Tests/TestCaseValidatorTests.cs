using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseValidatorTests
    {
        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public void ShouldRunVerifyMethodAndSucceedWithGoodTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = subject.Validate(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void ShouldRunVerifyMethodAndFailWithBadTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = subject.Validate(GetResultTestCase(supplied));

            Assert.AreEqual(Disposition.Failed, result.Result);
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = shouldPass   // Says the test Disposition.Passed
            };
        }
    }
}
