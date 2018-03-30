using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.HMAC
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IHmacFactory _algoFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IHmacFactory algoFactory)
        {
            _algoFactory = algoFactory;
            _random800_90 = random800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var algo = _algoFactory.GetHmacInstance(new HashFunction(testGroup.ShaMode, testGroup.ShaDigestSize));
            
            return new TestCaseGenerator(_random800_90, algo);
        }
    }
}
