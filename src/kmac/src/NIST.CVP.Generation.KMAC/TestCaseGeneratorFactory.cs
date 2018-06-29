using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Crypto.Common.Hash.CSHAKE;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IKmacFactory _algoFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IKmacFactory algoFactory)
        {
            _algoFactory = algoFactory;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var algo = _algoFactory.GetKmacInstance(testGroup.DigestSize * 2, testGroup.XOF);

            if (testGroup.TestType.ToLower() == "aft")
            {
                return new TestCaseGeneratorAFT(_random800_90, algo);
            }
            else if (testGroup.TestType.ToLower() == "vmt")
            {
                return new TestCaseGeneratorVMT(_random800_90, algo);
            }

            return new TestCaseGeneratorNull();
        }
    }
}
