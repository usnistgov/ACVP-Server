using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_CFBP;
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

            if (@group.TestType.ToLower() == "multiblockmessage")
            {
                if (@group.Function.ToLower() == "encrypt")
                {
                    return new TestCaseGeneratorMMTEncrypt(_random800_90, _modeOfOperation);
                }

                if (@group.Function.ToLower() == "decrypt")
                {
                    return new TestCaseGeneratorMMTDecrypt(_random800_90, _modeOfOperation);
                }
            }
            if (@group.TestType.ToLower() == "mct")
            {
                if (@group.Function.ToLower() == "encrypt")
                {
                    return new TestCaseGeneratorMonteCarloEncrypt(_random800_90, _modeOfOperationMCT);
                }
                if (@group.Function.ToLower() == "decrypt") 
                {
                    return new TestCaseGeneratorMonteCarloDecrypt(_random800_90, _modeOfOperationMCT);
                }
            }

            return new TestCaseGeneratorNull();
        }
    }
}