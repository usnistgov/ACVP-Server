﻿using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseValidatorNull : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly string _errorMessage;

        public int TestCaseId { get; }

        public TestCaseValidatorNull(string errorMessage, int testCaseId)
        {
            _errorMessage = errorMessage;
            TestCaseId = testCaseId;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            return new TestCaseValidation { Reason = _errorMessage, Result = Core.Enums.Disposition.Failed, TestCaseId = TestCaseId };
        }
    }
}