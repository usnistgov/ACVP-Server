using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public interface ITestCaseGeneratorFactory
    {
        GenerateResponse BuildTestCases(ITestVectorSet vectorSet);
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group, bool isSample);
    }
}
