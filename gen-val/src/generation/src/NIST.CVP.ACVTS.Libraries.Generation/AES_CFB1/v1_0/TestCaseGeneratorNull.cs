﻿using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;

namespace NIST.CVP.ACVTS.Libraries.Generation.AES_CFB1.v1_0
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
