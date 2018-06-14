using System;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate => throw new NotImplementedException();

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            throw new NotImplementedException();
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            throw new NotImplementedException();
        }
    }
}
