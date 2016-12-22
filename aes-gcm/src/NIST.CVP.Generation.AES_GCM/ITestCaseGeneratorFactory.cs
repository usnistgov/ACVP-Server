using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(string direction, string ivGen);
    }
}