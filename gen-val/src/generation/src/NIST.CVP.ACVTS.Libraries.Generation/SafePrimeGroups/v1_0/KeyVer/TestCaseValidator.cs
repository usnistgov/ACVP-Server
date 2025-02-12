﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            if (_expectedResult.TestPassed != suppliedResult.TestPassed)
            {
                var expected = new Dictionary<string, string>
                {
                    { nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString() }
                };

                var provided = new Dictionary<string, string>
                {
                    { nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.Value.ToString() }
                };

                return Task.FromResult(new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Disposition.Failed,
                    Reason = "Key should not have validated successfully.",
                    Expected = expected,
                    Provided = provided
                });
            }

            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = suppliedResult.TestCaseId,
                Result = Disposition.Passed
            });
        }
    }
}
