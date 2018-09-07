using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IKdfFactory _kdfFactory;

        public TestGroupGeneratorFactory(IKdfFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator(_kdfFactory)
            };

            return list;
        }
    }
}
