using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseValidatorNull : ITestCaseValidator<TestCase>
    {
        private readonly ITestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorNull(ITestCase testCase)
        {
            _expectedResult = testCase;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            return new TestCaseValidation { TestCaseId = TestCaseId, Result = Disposition.Failed, Reason = "Test type was not found" };
        }
    }
}
