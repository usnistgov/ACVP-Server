using NIST.CVP.Crypto.Common.KDF.Components.TLS;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.TLS
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ITlsKdfFactory _tlsKdfFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, ITlsKdfFactory tlsKdfFactory)
        {
            _random800_90 = random800_90;
            _tlsKdfFactory = tlsKdfFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var tls = _tlsKdfFactory.GetTlsKdfInstance(testGroup.TlsMode, testGroup.HashAlg);
            return new TestCaseGenerator(_random800_90, tls);
        }
    }
}
