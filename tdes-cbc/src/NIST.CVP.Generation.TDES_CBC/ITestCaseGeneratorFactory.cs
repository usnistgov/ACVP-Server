using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup @group, bool isSample);
    }
}