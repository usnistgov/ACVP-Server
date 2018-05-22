using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap.TDES
{
    public class TestCaseGeneratorNull : ITestCaseGenerator<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate => 1;
        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            return new TestCaseGenerateResponse("Null generator");
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse("Null generator");
        }
    }
}