using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_ECB
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup @group);
    }
}