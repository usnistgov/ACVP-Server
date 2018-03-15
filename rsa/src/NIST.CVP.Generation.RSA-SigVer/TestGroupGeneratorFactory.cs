using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IRandom800_90 _rand;
        private readonly IKeyBuilder _keyBuilder;
        private readonly IKeyComposerFactory _keyComposerFactory;
        private readonly IShaFactory _shaFactory;

        public TestGroupGeneratorFactory(IRandom800_90 rand, IKeyBuilder keyBuilder, IKeyComposerFactory keyComposerFactory, IShaFactory shaFactory)
        {
            _rand = rand;
            _keyBuilder = keyBuilder;
            _keyComposerFactory = keyComposerFactory;
            _shaFactory = shaFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGeneratorGeneratedDataTest(_rand, _keyBuilder, _keyComposerFactory, _shaFactory)
            };

            return list;
        }
    }
}
