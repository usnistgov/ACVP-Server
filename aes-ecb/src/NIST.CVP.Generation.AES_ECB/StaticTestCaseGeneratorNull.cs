using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.AES_ECB
{
    public class StaticTestCaseGeneratorNull : IStaticTestCaseGenerator<TestGroup, IEnumerable<TestCase>>
    {
        public MultipleTestCaseGenerateResponse<IEnumerable<TestCase>> Generate(TestGroup testGroup)
        {
            return new MultipleTestCaseGenerateResponse<IEnumerable<TestCase>>("This is the null generator -- nothing is generated");
        }
    }
}