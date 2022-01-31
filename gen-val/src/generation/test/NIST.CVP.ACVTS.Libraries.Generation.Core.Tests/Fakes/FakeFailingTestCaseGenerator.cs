using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeFailingTestCaseGenerator<TTestGroup, TTestCase> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>, new()
    {
        public int NumberOfTestCasesToGenerate => 1;
        public Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup group, bool isSample, int caseNo = 0)
        {
            return Task.FromResult(new TestCaseGenerateResponse<TTestGroup, TTestCase>("Fail"));
        }
    }
}
