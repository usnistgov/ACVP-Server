using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseValidatorHash : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorHash(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId
        {
            get { return _expectedResult.TestCaseId; }
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            if (_expectedResult.FailureTest)
            {
                if (!suppliedResult.FailureTest)
                {
                    errors.Add("Expected validation failure");
                }
            }
            else
            {
                if (!_expectedResult.Digest.Equals(suppliedResult.Digest))
                {
                    errors.Add("Digest does not match");
                }
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = "failed",
                    Reason = string.Join("; ", errors)
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }
    }
}
