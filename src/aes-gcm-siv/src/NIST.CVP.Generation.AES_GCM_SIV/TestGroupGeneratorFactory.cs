using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.AES_GCM_SIV
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var generators = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator()
            };

            return generators;
        }
    }
}
