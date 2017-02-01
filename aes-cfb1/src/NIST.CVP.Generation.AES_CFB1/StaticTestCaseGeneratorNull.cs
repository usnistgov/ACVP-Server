using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CFB1
{
    public class StaticTestCaseGeneratorNull : IStaticTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            return new MultipleTestCaseGenerateResponse<TestCase>("This is the null generator -- nothing is generated");
        }
    }
}