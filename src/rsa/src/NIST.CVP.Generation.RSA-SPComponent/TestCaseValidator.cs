﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SPComponent
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            if (suppliedResult.TestPassed == true)
            {
                if(suppliedResult.Signature == null)
                {
                    errors.Add("Could not find signature");
                }
                else
                {
                    if (!_expectedResult.Signature.Equals(suppliedResult.Signature))
                    {
                        errors.Add("Signature does not match expected value");
                        expected.Add(nameof(_expectedResult.Signature), _expectedResult.Signature.ToHex());
                        provided.Add(nameof(suppliedResult.Signature), suppliedResult.Signature.ToHex());
                    }
                }
            }
            else
            {
                if (_expectedResult.TestPassed != suppliedResult.TestPassed)
                {
                    if (_expectedResult.TestPassed == false)
                    {
                        errors.Add("Test was expected to fail");
                    }
                    else
                    {
                        errors.Add("Test was not expected to fail");
                    }
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                { 
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join(";", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}