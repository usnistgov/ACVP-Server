using NIST.CVP.Crypto.Common.KDF.Components.SRTP;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SRTP
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISrtpFactory _srtpFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISrtpFactory srtpFactory)
        {
            _random800_90 = random800_90;
            _srtpFactory = srtpFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var srtp = _srtpFactory.GetInstance();

            return new TestCaseGenerator(_random800_90, srtp);
        }
    }
}
