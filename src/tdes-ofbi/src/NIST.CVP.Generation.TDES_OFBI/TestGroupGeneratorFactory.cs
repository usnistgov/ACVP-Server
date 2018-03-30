using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            var list = new HashSet<ITestGroupGenerator<Parameters>>
                {
                    new TestGroupGeneratorKnownAnswer(),
                    new TestGroupGeneratorMultiblockMessage(),
                    new TestGroupGeneratorMonteCarlo()
                };

            return list;
        }
    }
}
