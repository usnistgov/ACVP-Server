using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CMAC_AES
{
    public class TestCaseGeneratorNull : ITestCaseGenerator<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get { return 0; } }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            return Generate(group, null);
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse("This is the null generator -- nothing is generated");
        }
    }
}
