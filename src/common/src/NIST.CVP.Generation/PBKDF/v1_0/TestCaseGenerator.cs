using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.PBKDF.v1_0
{
    public class TestCaseGenerator : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        
        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup @group, bool isSample)
        {
            throw new System.NotImplementedException();
        }
    }
}