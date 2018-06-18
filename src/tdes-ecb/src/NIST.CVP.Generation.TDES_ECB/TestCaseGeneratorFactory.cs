using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using AlgoArrayResponse = NIST.CVP.Crypto.Common.Symmetric.TDES.AlgoArrayResponse;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IModeBlockCipher<SymmetricCipherResult> _algo;
        private readonly IMonteCarloTester<MCTResult<AlgoArrayResponse>, AlgoArrayResponse> _mctAlgo;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IBlockCipherEngineFactory engineFactory, IModeBlockCipherFactory cipherFactory, IMonteCarloFactoryTdes mctFactory)
        {
            _random800_90 = random800_90;
            _algo = cipherFactory.GetStandardCipher(engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes), BlockCipherModesOfOperation.Ecb);
            _mctAlgo = mctFactory.GetInstance(BlockCipherModesOfOperation.Ecb);
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
