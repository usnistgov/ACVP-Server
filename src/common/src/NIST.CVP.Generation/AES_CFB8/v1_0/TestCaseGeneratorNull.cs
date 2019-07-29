using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_CFB8.v1_0
{
    public class TestCaseGeneratorNull : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {

        public int NumberOfTestCasesToGenerate => 1;

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample, int caseNo = 0)
        {
            return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>(
                "This is the null generator -- nothing is generated"
            ));
        }
    }
}
