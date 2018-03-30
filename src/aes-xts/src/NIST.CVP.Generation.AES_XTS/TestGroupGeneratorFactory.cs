using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.AES_XTS
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
