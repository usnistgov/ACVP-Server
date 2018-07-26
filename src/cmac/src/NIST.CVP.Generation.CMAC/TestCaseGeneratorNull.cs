using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.CMAC
{
    public class TestCaseGeneratorNull<TTestGroup, TTestCase> : ITestCaseGeneratorAsync<TTestGroup, TTestCase>
        where TTestGroup : TestGroupBase<TTestGroup, TTestCase>
        where TTestCase : TestCaseBase<TTestGroup, TTestCase>, new()
    {
        public int NumberOfTestCasesToGenerate => 0;

        public Task<TestCaseGenerateResponse<TTestGroup, TTestCase>> GenerateAsync(TTestGroup @group, bool isSample)
        {
            return Task.FromResult(
                new TestCaseGenerateResponse<TTestGroup, TTestCase>(
                    "This is the null generator -- nothing is generated"));
        }
    }
}
