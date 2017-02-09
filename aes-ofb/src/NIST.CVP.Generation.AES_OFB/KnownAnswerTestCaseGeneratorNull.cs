using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_OFB
{
    public class KnownAnswerTestCaseGeneratorNull : IKnownAnswerTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            return new MultipleTestCaseGenerateResponse<TestCase>("This is the null generator -- nothing is generated");
        }
    }
}