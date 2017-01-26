using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA1
{
    public class TestCaseValidatorMCTHash : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorMCTHash(TestCase expectedResult)
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
            for(int i = 0; i < _expectedResult.ResultsArray.Count; i++)
            {
                if (!_expectedResult.ResultsArray[i].Message.Equals(suppliedResult.ResultsArray[i].Message))
                {
                    errors.Add($"Message does not match on iteration {i}");
                }

                if (!_expectedResult.ResultsArray[i].Digest.Equals(suppliedResult.ResultsArray[i].Digest))
                {
                    errors.Add($"Digest does not match on iteration {i}");
                }
            }

            if(errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }
    }
}
