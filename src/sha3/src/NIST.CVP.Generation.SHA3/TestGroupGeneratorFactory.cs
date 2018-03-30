using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters>
    {
        public IEnumerable<ITestGroupGenerator<Parameters>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<Parameters>> list =
                new HashSet<ITestGroupGenerator<Parameters>>()
                {
                    new TestGroupGeneratorAlgorithmFunctional(),
                    new TestGroupGeneratorMonteCarlo(),
                    new TestGroupGeneratorVariableOutput()
                };

            return list;
        }
    }
}
