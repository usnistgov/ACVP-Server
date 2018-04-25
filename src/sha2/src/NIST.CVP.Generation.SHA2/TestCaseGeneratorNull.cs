using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestCaseGeneratorNull : ITestCaseGenerator<TestGroup, TestCase>
    {
        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, bool isSample)
        {
            return Generate(group, null);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup @group, TestCase testCase)
        {
            return new TestCaseGenerateResponse<TestGroup, TestCase>("This is the null generator -- nothing is generated");
        }

        public int NumberOfTestCasesToGenerate { get; set; }
    }
}
