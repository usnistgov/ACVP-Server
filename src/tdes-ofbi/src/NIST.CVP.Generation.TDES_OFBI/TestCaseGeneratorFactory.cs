using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITDES_OFBI _algo;
        private readonly ITDES_OFBI_MCT _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ITDES_OFBI algo, ITDES_OFBI_MCT mctAlgo)
        {
            _algo = algo;
            _random800_90 = random800_90;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            switch (group.TestType.ToLower())
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKnownAnswer(group);

                case "multiblockmessage":
                    switch (group.Function.ToLower())
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _algo);
                    }

                    break;

                case "mct":
                    switch (group.Function.ToLower())
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMonteCarloEncrypt(_random800_90, _mctAlgo);
                        case "decrypt":
                            return new TestCaseGeneratorMonteCarloDecrypt(_random800_90, _mctAlgo);
                    }

                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
