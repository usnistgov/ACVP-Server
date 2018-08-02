using System.Threading.Tasks;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeTestCaseGenerator<TTestGroup, TTestCase> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>, new()
    {
        public int NumberOfTestCasesToGenerate => 1;
        public Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample)
        {
            var testCase = new TTestCase();
            return Task.FromResult(new TestCaseGenerateResponse<TTestGroup, TTestCase>(testCase));
        }
    }
}
