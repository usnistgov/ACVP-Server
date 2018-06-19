using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseValidatorVer<TTestGroup, TTestCase> : ITestCaseValidator<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>
    {
        private readonly TTestCase _expectedResult;

        public TestCaseValidatorVer(TTestCase expectedResult)
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
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }
            return  new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }

        private void ValidateResultPresent(TTestCase suppliedResult, List<string> errors)
        {
            if (suppliedResult.TestPassed == null)
            {
                errors.Add($"{nameof(suppliedResult.TestPassed)} was not present in the {nameof(TTestCase)}");
            }
        }

        private void CheckResults(TTestCase suppliedResult, List<string> errors, Dictionary<string, string> expected, Dictionary<string, string> provided)
        {
            if (!_expectedResult.TestPassed.Equals(suppliedResult.TestPassed))
            {
                errors.Add("Result does not match");
                expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString());
                provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.Value.ToString());
            }
        }
    }
}
