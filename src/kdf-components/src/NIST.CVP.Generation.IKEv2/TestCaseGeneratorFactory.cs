using NIST.CVP.Crypto.Common.KDF.Components.IKEv2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.IKEv2
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IIkeV2Factory _ikeV2Factory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IIkeV2Factory ikeV2Factory)
        {
            _random800_90 = random800_90;
            _ikeV2Factory = ikeV2Factory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var ikeV2 = _ikeV2Factory.GetInstance(testGroup.HashAlg);
            return new TestCaseGenerator(_random800_90, ikeV2);
        }
    }
}
