using NIST.CVP.Crypto.Common.KDF.Components.SSH;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SSH
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISshFactory _sshFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ISshFactory sshFactory)
        {
            _random800_90 = random800_90;
            _sshFactory = sshFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var ssh = _sshFactory.GetSshInstance(testGroup.HashAlg, testGroup.Cipher);
            return new TestCaseGenerator(_random800_90, ssh);
        }
    }
}
