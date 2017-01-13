using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public class StaticTestCaseGeneratorFactory : IStaticTestCaseGeneratorFactory<TestGroup, IEnumerable<TestCase>>
    {
        public IStaticTestCaseGenerator<TestGroup, IEnumerable<TestCase>> GetStaticCaseGenerator(string direction, string testType)
        {
            direction = direction.ToLower();
            testType = testType.ToLower();

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
