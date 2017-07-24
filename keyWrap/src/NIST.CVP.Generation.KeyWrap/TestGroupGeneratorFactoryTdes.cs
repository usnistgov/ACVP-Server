using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KeyWrap
{
    public class TestGroupGeneratorFactoryTdes : ITestGroupGeneratorFactory<ParametersTdes>
    {
        public IEnumerable<ITestGroupGenerator<ParametersTdes>> GetTestGroupGenerators()
        {
            HashSet<ITestGroupGenerator<ParametersTdes>> list =
                new HashSet<ITestGroupGenerator<ParametersTdes>>()
                {
                    new TestGroupGeneratorTdes()
                };

            return list;
        }
    }
}