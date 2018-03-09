using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ICFBPMode _modeOfOperation;
        private readonly ICFBPModeMCT _modeOfOperationMCT;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ICFBPMode modeOfOperation, ICFBPModeMCT modeOfOperationMCT)
        {
            _random800_90 = random800_90;
            _modeOfOperation = modeOfOperation;
            _modeOfOperationMCT = modeOfOperationMCT;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup @group)
        {
            switch (@group.TestType.ToLower())
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKnownAnswer(group, _modeOfOperation.Algo);

                case "multiblockmessage":
                    switch (@group.Function.ToLower())
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _modeOfOperation);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _modeOfOperation);
                    }

                    break;

                case "mct":
                    switch (@group.Function.ToLower())
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMonteCarloEncrypt(_random800_90, _modeOfOperationMCT);
                        case "decrypt":
                            return new TestCaseGeneratorMonteCarloDecrypt(_random800_90, _modeOfOperationMCT);
                    }

                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}