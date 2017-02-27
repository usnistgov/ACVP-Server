using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public interface ITestCaseGeneratorFactory
    {
        GenerateResponse BuildTestCases(ITestVectorSet vectorSet);
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group, bool isSample);
    }
}
