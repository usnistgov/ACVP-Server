using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestCaseValidatorHash : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorHash(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors, expected, provided);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation 
                { 
                    TestCaseId = suppliedResult.TestCaseId, 
                    Result = Core.Enums.Disposition.Failed, 
                    Reason = string.Join("; ", errors),
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation {TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed};
        }

        private void ValidateResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Digest == null)
            {
                errors.Add($"{nameof(suppliedResult.Digest)} was not present in the {nameof(TestCase)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.Digest.Equals(suppliedResult.Digest))
            {
                errors.Add("Digests do not match");
                expected.Add(nameof(_expectedResult.Digest), _expectedResult.Digest.ToHex());
                provided.Add(nameof(suppliedResult.Digest), suppliedResult.Digest.ToHex());
            }
        }
    }
}
