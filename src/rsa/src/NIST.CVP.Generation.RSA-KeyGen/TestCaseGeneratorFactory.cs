using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorFactory : ITestCaseGeneratorFactory<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public TestCaseGeneratorFactory(IRandom800_90 random800_90, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IShaFactory shaFactory)
        {
            _random800_90 = random800_90;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
        {
            switch (testGroup.TestType.ToLower())
            {
                case "kat":
                    return new TestCaseGeneratorKat(testGroup, _keyComposerFactory);

                case "aft":
                case "gdt":
                    // Aft and Gdt generator would do the same function (validators differ) so they are lumped together
                    return new TestCaseGeneratorAft(_random800_90, _keyBuilder, _keyComposerFactory, _shaFactory);

                default:
                    return new TestCaseGeneratorNull();
            }
        }
    }
}
