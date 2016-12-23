using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(string direction);
    }
}