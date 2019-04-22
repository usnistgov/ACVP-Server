using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.AES_CCM.v1_0
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
