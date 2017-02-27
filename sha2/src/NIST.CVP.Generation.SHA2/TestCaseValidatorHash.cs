using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
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
            if (!_expectedResult.Digest.Equals(suppliedResult.Digest))
            {
                errors.Add("Digests do not match");
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors)};
            }

            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = "passed"};
        }
    }
}
