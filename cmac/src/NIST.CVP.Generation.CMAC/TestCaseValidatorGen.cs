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

        public TestCaseValidation Validate(TTestCase suppliedResult)
        {
            var errors = new List<string>();

            ValidateResultPresent(suppliedResult, errors);
            if (errors.Count == 0)
            {
                CheckResults(suppliedResult, errors);
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = string.Join("; ", errors) };
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

        private void CheckResults(TTestCase suppliedResult, List<string> errors)
        {
            if (!_expectedResult.Mac.Equals(suppliedResult.Mac))
            {
                errors.Add("MAC does not match");
            }
        }
    }
}
