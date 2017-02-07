using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group, bool isSample);
    }
}
