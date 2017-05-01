using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseValidatorMCTHash : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorMCTHash(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            ValidateArrayResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed" };
        }

        private void ValidateArrayResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ResultsArray == null || suppliedResult.ResultsArray.Count == 0)
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.ResultsArray.Any(a => a.Message == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Message)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.Digest == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponse.Digest)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors)
        {
            for (var i = 0; i < _expectedResult.ResultsArray.Count; i++)
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
        }
    }
}
