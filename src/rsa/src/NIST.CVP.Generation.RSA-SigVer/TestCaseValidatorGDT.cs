using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseValidatorGDT : ITestCaseValidator<TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidatorGDT(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.Result == suppliedResult.Result)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
            }
            else
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Failed, Reason = _expectedResult.Reason.GetName() };
            }
        }
    }
}
