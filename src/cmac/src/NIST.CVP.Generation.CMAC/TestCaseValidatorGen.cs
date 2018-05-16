using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseValidatorGen<TTestGroup, TTestCase> : ITestCaseValidator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        private readonly TTestCase _expectedResult;

        public TestCaseValidatorGen(TTestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidation Validate(TTestCase suppliedResult, bool showExpected = false)
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
                    Expected = showExpected ? expected : null,
                    Provided = showExpected ? provided : null
                };
            }
            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.Mac == null)
            {
                errors.Add($"{nameof(suppliedResult.Mac)} was not present in the {typeof(TTestCase)}");
            }
        }

        private void CheckResults(TTestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.Mac.Equals(suppliedResult.Mac))
            {
                errors.Add("MAC does not match");
                expected.Add(nameof(_expectedResult.Mac), _expectedResult.Mac.ToHex());
                provided.Add(nameof(suppliedResult.Mac), suppliedResult.Mac.ToHex());
            }
        }
    }
}
