﻿using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_CTR.v1_0
{
    public class TestCaseValidatorNull : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorNull(TestCase testCase)
        {
            _expectedResult = testCase;
        }

        public async Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            return await Task.FromResult(new TestCaseValidation
            {
                TestCaseId = TestCaseId,
                Result = Disposition.Failed,
                Reason = "Test type was not found"
            });
        }
    }
}