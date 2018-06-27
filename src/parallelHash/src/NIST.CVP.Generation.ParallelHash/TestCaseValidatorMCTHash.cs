using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestCaseValidatorMCTHash : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorMCTHash(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            var errors = new List<string>();
            var expected = new Dictionary<string, string>();
            var provided = new Dictionary<string, string>();

            ValidateArrayResultPresent(suppliedResult, errors);
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

        private void ValidateArrayResultPresent(TestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.ResultsArray == null || suppliedResult.ResultsArray.Count == 0)
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} was not present in the {nameof(TestCase)}");
                return;
            }

            if (suppliedResult.ResultsArray.Any(a => a.Message == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithCustomization.Message)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.Digest == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithCustomization.Digest)}");
            }
            if (suppliedResult.ResultsArray.Any(a => a.Customization == null))
            {
                errors.Add($"{nameof(suppliedResult.ResultsArray)} did not contain expected element {nameof(AlgoArrayResponseWithCustomization.Customization)}");
            }
        }

        private void CheckResults(TestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            for (var i = 0; i < _expectedResult.ResultsArray.Count; i++)
            {
                if (!_expectedResult.ResultsArray[i].Message.Equals(suppliedResult.ResultsArray[i].Message))
                {
                    errors.Add($"Message does not match on iteration {i}");
                    expected.Add($"Message {i}", _expectedResult.ResultsArray[i].Message.ToHex());
                    provided.Add($"Message {i}", suppliedResult.ResultsArray[i].Message.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].Digest.Equals(suppliedResult.ResultsArray[i].Digest))
                {
                    errors.Add($"Digest does not match on iteration {i}");
                    expected.Add($"Digest {i}", _expectedResult.ResultsArray[i].Digest.ToHex());
                    provided.Add($"Digest {i}", suppliedResult.ResultsArray[i].Digest.ToHex());
                }
                if (!_expectedResult.ResultsArray[i].Customization.Equals(suppliedResult.ResultsArray[i].Customization))
                {
                    errors.Add($"Customization does not match on iteration {i}");
                    expected.Add($"Customization {i}", _expectedResult.ResultsArray[i].Customization);
                    provided.Add($"Customization {i}", suppliedResult.ResultsArray[i].Customization);
                }
            }
        }
    }
}
