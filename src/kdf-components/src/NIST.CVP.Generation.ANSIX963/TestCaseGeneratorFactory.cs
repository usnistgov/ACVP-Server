using NIST.CVP.Crypto.Common.KDF.Components.AnsiX963;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.ANSIX963
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IAnsiX963Factory _ansiX963Factory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IAnsiX963Factory ansiX963Factory)
        {
            _random800_90 = random800_90;
            _ansiX963Factory = ansiX963Factory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            var algo = _ansiX963Factory.GetInstance(testGroup.HashAlg);
            return new TestCaseGenerator(_random800_90, algo);
        }
    }
}
