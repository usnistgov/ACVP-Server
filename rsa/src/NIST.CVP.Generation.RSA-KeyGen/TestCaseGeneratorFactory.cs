using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90)
        {
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            switch (testGroup.Mode)
            {
                case KeyGenModes.B32:
                    return new TestCaseGeneratorAFT_B32(_random800_90, new RandomProvablePrimeGenerator());

                case KeyGenModes.B33:
                    switch (testGroup.TestType.ToLower())
                    {
                        case "kat":
                            return new KnownAnswerTestCaseGeneratorB33(testGroup);
                        default:
                            return new TestCaseGeneratorGDT_B33(_random800_90, new RandomProbablePrimeGenerator());
                    }

                case KeyGenModes.B34:
                    return new TestCaseGeneratorAFT_B34(_random800_90, new AllProvablePrimesWithConditionsGenerator());

                case KeyGenModes.B35:
                    return new TestCaseGeneratorAFT_B35(_random800_90, new ProvableProbablePrimesWithConditionsGenerator());

                case KeyGenModes.B36:
                    return new TestCaseGeneratorAFT_B36(_random800_90, new AllProbablePrimesWithConditionsGenerator());

                default:
                    return new TestCaseGeneratorNull();
            }
        }
    }
}
