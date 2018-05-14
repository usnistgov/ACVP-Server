using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.AES_CTR
{
    public class TestCaseValidatorNull : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorNull(TestCase testCase)
        {
            _expectedResult = testCase;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            return new TestCaseValidation { TestCaseId = TestCaseId, Result = Disposition.Failed, Reason = "Test type was not found"};
        }
    }
}
