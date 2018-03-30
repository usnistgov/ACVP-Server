using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS.ECC
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGenerator(),
                };

            return list;
        }
    }
}