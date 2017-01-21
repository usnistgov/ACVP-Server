using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public class StaticTestCaseGeneratorFactory : IStaticTestCaseGeneratorFactory<TestGroup, TestCase>
    {
        public IStaticTestCaseGenerator<TestGroup, TestCase> GetStaticCaseGenerator(TestGroup testGroup)
        {
            var testType = testGroup.TestType.ToLower();

            switch (testType)
            {
                case "gfsbox":
                    return new StaticTestCaseGeneratorGFSBox();
                case "keysbox":
                    return new StaticTestCaseGeneratorKeySBox();
                case "vartxt":
                    return new StaticTestCaseGeneratorVarTxt();
                case "varkey":
                    return new StaticTestCaseGeneratorVarKey();
            }

            return new StaticTestCaseGeneratorNull();
        }
    }
}
