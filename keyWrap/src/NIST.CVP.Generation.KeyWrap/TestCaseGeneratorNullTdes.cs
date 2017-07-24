using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorNullTdes : ITestCaseGenerator<TestGroupTdes, TestCaseTdes>
    {
        public int NumberOfTestCasesToGenerate => 1;
        public TestCaseGenerateResponse Generate(TestGroupTdes @group, bool isSample)
        {
            return new TestCaseGenerateResponse("Null generator");
        }

        public TestCaseGenerateResponse Generate(TestGroupTdes @group, TestCaseTdes testCase)
        {
            return new TestCaseGenerateResponse("Null generator");
        }
    }
}