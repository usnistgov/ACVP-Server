using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeTestCaseValidator<TTestGroup, TTestCase> : ITestCaseValidatorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public int TestCaseId { get; set; }
        private readonly Disposition _result;
        public FakeTestCaseValidator(Disposition result)
        {
            _result = result;
        }

        public Task<TestCaseValidation> ValidateAsync(TTestCase suppliedResult, bool showExpected)
        {
            return Task.FromResult(new TestCaseValidation { TestCaseId = TestCaseId, Result = _result });
        }
    }
}
