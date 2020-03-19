using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM_SIV.v1_0
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var generators = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator()
            };

            return generators;
        }
    }
}
