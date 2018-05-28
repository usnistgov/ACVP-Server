using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IBlockCipherEngineFactory _engineFactory;
        private readonly IModeBlockCipherFactory _modeFactory;
        private readonly ICounterFactory _counterFactory;

        public TestCaseGeneratorFactory(
            IRandom800_90 random800_90, 
            IBlockCipherEngineFactory engineFactory, 
            IModeBlockCipherFactory modeFactory, 
            ICounterFactory counterFactory
        )
        {
            _random800_90 = random800_90;
            _engineFactory = engineFactory;
            _modeFactory = modeFactory;
            _counterFactory = counterFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup group)
        {
            var testType = group.TestType.ToLower();
            var direction = group.Direction.ToLower();

            switch (testType)
            {
                case "permutation":
                case "inversepermutation":
                case "substitutiontable":
                case "variablekey":
                case "variabletext":
                    return new TestCaseGeneratorKnownAnswer(group);

                case "singleblock":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorSingleBlockEncrypt(_random800_90, _engineFactory, _modeFactory, _counterFactory);
                        case "decrypt":
                            return new TestCaseGeneratorSingleBlockDecrypt(_random800_90, _engineFactory, _modeFactory, _counterFactory);
                    }

                    break;

                case "partialblock":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorPartialBlockEncrypt(_random800_90, _engineFactory, _modeFactory, _counterFactory);
                        case "decrypt":
                            return new TestCaseGeneratorPartialBlockDecrypt(_random800_90, _engineFactory, _modeFactory, _counterFactory);
                    }

                    break;

                case "counter":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorCounterEncrypt(_random800_90, _engineFactory, _modeFactory, _counterFactory);
                        case "decrypt":
                            return new TestCaseGeneratorCounterDecrypt(_random800_90, _engineFactory, _modeFactory, _counterFactory);
                    }

                    break;
            }

            return new TestCaseGeneratorNull();
        }
    }
}
