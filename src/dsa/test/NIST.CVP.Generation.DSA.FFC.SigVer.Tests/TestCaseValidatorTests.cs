﻿using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.Tests
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

            Assert.AreEqual(Core.Enums.Disposition.Passed, result.Result);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        public void ShouldRunVerifyMethodAndFailWithBadTest(bool expected, bool supplied)
        {
            var subject = new TestCaseValidator(GetResultTestCase(expected));
            var result = subject.Validate(GetResultTestCase(supplied));

            Assert.AreEqual(Core.Enums.Disposition.Failed, result.Result);
        }

        private TestCase GetResultTestCase(bool shouldPass)
        {
            return new TestCase
            {
                TestCaseId = 1,
                TestPassed = shouldPass,   // Says the test Core.Enums.Disposition.Passed
                Reason = new TestCaseExpectationReason(SigFailureReasons.ModifyMessage)     // Only matters in the failure event
            };
        }
    }
}