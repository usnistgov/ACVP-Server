﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0
{
    public class TestCaseValidatorNull : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorNull(TestCase testCase)
        {
            _expectedResult = testCase;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            return Task.FromResult(new TestCaseValidation
            {
                TestCaseId = TestCaseId,
                Result = Disposition.Failed,
                Reason = "Test type was not found"
            });
        }
    }
}
