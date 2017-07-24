using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class KnownAnswerTestCaseGeneratorFactory : IKnownAnswerTestCaseGeneratorFactory<TestGroup, TestCase>
    {
        public IKnownAnswerTestCaseGenerator<TestGroup, TestCase> GetStaticCaseGenerator(TestGroup testGroup)
        {
            switch (testGroup.Mode)
            {
                case KeyGenModes.B33:
                    return new KnownAnswerTestCaseGeneratorB33();

                default:
                    return new KnownAnswerTestCaseGeneratorNull();
            }
        }
    }
}
