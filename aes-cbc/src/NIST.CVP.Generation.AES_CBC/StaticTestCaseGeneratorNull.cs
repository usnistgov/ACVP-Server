using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_CBC
{
    public class StaticTestCaseGeneratorNull : IStaticTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            return new MultipleTestCaseGenerateResponse<TestCase>("This is the null generator -- nothing is generated");
        }
    }
}