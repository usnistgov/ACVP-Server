using NIST.CVP.Crypto.DRBG;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.DRBG
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {

        private readonly IEntropyProviderFactory _iEntropyProviderFactory;
        private readonly IDrbgFactory _iDrbgFactory;

        public TestCaseGeneratorFactory(IEntropyProviderFactory iEntropyProviderFactory, IDrbgFactory iDrbgFactory)
        {
            _iEntropyProviderFactory = iEntropyProviderFactory;
            _iDrbgFactory = iDrbgFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            if (testGroup.ReSeed)
            {
                if (testGroup.PredResistance)
                {
                    return new TestCaseGeneratorReseedPredResist(_iEntropyProviderFactory, _iDrbgFactory);
                }
                
                return new TestCaseGeneratorReseedNoPredResist(_iEntropyProviderFactory, _iDrbgFactory);
            }

            return new TestCaseGeneratorNoReseed(_iEntropyProviderFactory, _iDrbgFactory);
        }
    }
}
