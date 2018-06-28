using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators()
        {
            var list =
                new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGeneratorAlgorithmFunctional(),
                    new TestGroupGeneratorMonteCarlo(),
                    new TestGroupGeneratorVariableOutput(),
                    new TestGroupGeneratorLargeCustomization()
                };

            return list;
        }
    }
}
