using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC
{
    public class KnownAnswerTestCaseGeneratorFactory : IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase>
    {
        public IKnownAnswerTestCaseGenerator<TestGroup, TestCase> GetStaticCaseGenerator(TestGroup testGroup)
        {
            var testType = testGroup.TestType.ToLower();

            switch (testType)
            {
                case "gfsbox":
                    return new KnownAnswerTestCaseGeneratorGFSBox();
                case "keysbox":
                    return new KnownAnswerTestCaseGeneratorKeySBox();
                case "vartxt":
                    return new KnownAnswerTestCaseGeneratorVarTxt();
                case "varkey":
                    return new KnownAnswerTestCaseGeneratorVarKey();
            }

            return new KnownAnswerTestCaseGeneratorNull();
        }
    }
}
