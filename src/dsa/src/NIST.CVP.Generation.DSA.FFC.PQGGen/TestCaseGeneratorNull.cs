using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestCaseGeneratorNull : ITestCaseGeneratorAsync<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate => 1;

        public Task<TestCaseGenerateResponse<TestGroup, TestCase>> GenerateAsync(TestGroup group, bool isSample)
        {
            return Task.FromResult(
                new TestCaseGenerateResponse<TestGroup, TestCase>(
                    "This is the null generator -- nothing is generated"));
        }
    }
}
