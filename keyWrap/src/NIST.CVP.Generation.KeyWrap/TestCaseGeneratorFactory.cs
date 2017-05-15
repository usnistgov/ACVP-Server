using NIST.CVP.Crypto.KeyWrap;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IKeyWrapFactory _iKeyWrapFactory;
        private readonly IRandom800_90 _iRandom800_90;

        public TestCaseGeneratorFactory(IKeyWrapFactory iKeyWrapFactory, IRandom800_90 iRandom800_90)
        {
            _iKeyWrapFactory = iKeyWrapFactory;
            _iRandom800_90 = iRandom800_90;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            switch (testGroup.Direction.ToLower())
            {
                case "encrypt":
                    return new TestCaseGeneratorEncrypt(_iKeyWrapFactory, _iRandom800_90);
                case "decrypt":
                    return new TestCaseGeneratorDecrypt(_iKeyWrapFactory, _iRandom800_90);
                default:
                    return new TestCaseGeneratorNull();
            }
        }
    }
}
