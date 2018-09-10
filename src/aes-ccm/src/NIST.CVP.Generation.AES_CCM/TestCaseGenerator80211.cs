using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES_CCM
{
    public class TestCaseGenerator80211 : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate => throw new System.NotImplementedException();

        public Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            throw new System.NotImplementedException();
        }
    }
}
