using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class KnownAnswerTestCaseGeneratorFactory : IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase>
    {
        public IKnownAnswerTestCaseGenerator<TestGroup, TestCase> GetStaticCaseGenerator(TestGroup testGroup)
        {
            switch (testGroup.TestType.ToLower())
            {
                case "gfsbox":
                    return new KnownAnswerTestCaseGeneratorGfSBox();
                case "keysbox":
                    return new KnownAnswerTestCaseGeneratorKeySBox();
                case "vartxt":
                    return new KnownAnswerTestCaseGeneratorVarTxt();
                case "varkey":
                    return new KnownAnswerTestCaseGeneratorVarKey();
                default:
                    return new KnownAnswerTestCaseGeneratorNull();
            }
        }
    }
}
