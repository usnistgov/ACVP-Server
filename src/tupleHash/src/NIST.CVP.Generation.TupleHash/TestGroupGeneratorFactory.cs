using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TupleHash
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
                    new TestGroupGeneratorLargeTuple()
                };

            return list;
        }
    }
}
