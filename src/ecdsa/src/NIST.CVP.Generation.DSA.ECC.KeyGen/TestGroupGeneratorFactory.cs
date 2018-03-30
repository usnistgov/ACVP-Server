using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
            {
                new TestGroupGenerator()
            };

            return list;
        }
    }
}
