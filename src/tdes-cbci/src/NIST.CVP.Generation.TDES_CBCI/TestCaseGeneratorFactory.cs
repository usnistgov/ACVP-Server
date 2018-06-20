using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.MonteCarlo;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CBCI
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IMonteCarloFactoryTdesPartitions _mctFactory;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;

        public TestCaseGeneratorFactory(
            IRandom800_90 random800_90,
            IMonteCarloFactoryTdesPartitions mctFactory,
            IBlockCipherEngineFactory engineFactory,
            IModeBlockCipherFactory modeFactory
        )
        {
            _random800_90 = random800_90;
            _mctFactory = mctFactory;
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
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
                    return new TestCaseGeneratorKnownAnswer(group);

                case "multiblockmessage":
                    switch (@group.Function.ToLower())
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _engineFactory, _modeFactory);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _engineFactory, _modeFactory);
                    }

                    break;

                case "mct":
                    switch (@group.Function.ToLower())
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMonteCarloEncrypt(_random800_90, _mctFactory);
                        case "decrypt":
                            return new TestCaseGeneratorMonteCarloDecrypt(_random800_90, _mctFactory);
                    }

                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
