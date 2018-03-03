using System.Collections.Generic;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        private readonly IKdfFactory _kdfFactory;

        public TestGroupGeneratorFactory(IKdfFactory kdfFactory)
        {
            _kdfFactory = kdfFactory;
        }

        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGenerator(_kdfFactory)
            };

            return list;
        }
    }
}
