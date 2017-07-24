using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class KnownAnswerTestCaseGeneratorNull : IKnownAnswerTestCaseGenerator<TestGroup, TestCase>
    {
        public MultipleTestCaseGenerateResponse<TestCase> Generate(TestGroup testGroup)
        {
            return new MultipleTestCaseGenerateResponse<TestCase>("This is the null generator -- nothing is generated");
        }
    }
}
