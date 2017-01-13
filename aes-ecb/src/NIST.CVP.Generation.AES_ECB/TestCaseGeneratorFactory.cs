using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAES_ECB _algo;
        private readonly IAES_ECB_MCT _mctAlgo;
        public const int _DEFAULT_NUMBER_TEST_CASES = 15; // @@@ TODO Make configurable?

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAES_ECB algo, IAES_ECB_MCT mctAlgo)
        {
            _algo = algo;
            _random800_90 = random800_90;
            _mctAlgo = mctAlgo;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(string direction, string testType)
        {
            direction = direction.ToLower();
            testType = testType.ToLower();

            switch (testType)
            {
                case "mct":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMCTEncrypt(_random800_90, _mctAlgo);
                        case "decrypt":
                            return new TestCaseGeneratorMCTDecrypt(_random800_90, _mctAlgo);
                    }
                    break;
                case "mmt":
                    switch (direction)
                    {
                        case "encrypt":
                            return new TestCaseGeneratorMMTEncrypt(_random800_90, _algo);
                        case "decrypt":
                            return new TestCaseGeneratorMMTDecrypt(_random800_90, _algo);
                    }
                    break;
            }

            return new TestCaseGeneratorNull();
        }

        public int NumberOfTestCases { get; private set; } = _DEFAULT_NUMBER_TEST_CASES;
    }
}
