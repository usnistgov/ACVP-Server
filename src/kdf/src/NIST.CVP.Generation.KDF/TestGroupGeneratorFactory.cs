using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KDF;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGenerator(new KdfFactory())
            };

            return list;
        }
    }
}
