using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.TDES_CFB.v1_0
{
    public class TestCaseGeneratorNull : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; set; }

        public async Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            return await Task.FromResult(new TestCaseGenerateResponse<TestGroup, TestCase>("This is the null generator -- nothing is generated"));
        }
    }
}