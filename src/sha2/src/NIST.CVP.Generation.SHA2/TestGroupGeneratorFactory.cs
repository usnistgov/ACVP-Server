using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA2
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorAlgorithmFunctionalTest(),
                    new TestGroupGeneratorMonteCarloTest()
                };

            return list;
        }
    }
}
