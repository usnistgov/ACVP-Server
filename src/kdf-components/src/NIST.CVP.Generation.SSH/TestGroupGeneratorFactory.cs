using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SSH
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator()
            };

            return list;
        }
    }
}
