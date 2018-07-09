using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IKeyBuilder _keyBuilder;
        private readonly IRandom800_90 _rand;
        private readonly IKeyComposerFactory _keyComposerFactory;

        public TestGroupGeneratorFactory(IKeyBuilder keyBuilder, IRandom800_90 rand, IKeyComposerFactory keyComposerFactory)
        {
            _keyBuilder = keyBuilder;
            _rand = rand;
            _keyComposerFactory = keyComposerFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGeneratorGeneratedDataTest(_keyBuilder, _rand, _keyComposerFactory)
            };

            return list;
        }
    }
}
