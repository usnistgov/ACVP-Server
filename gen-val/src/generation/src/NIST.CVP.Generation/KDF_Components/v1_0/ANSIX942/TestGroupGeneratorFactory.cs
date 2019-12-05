using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
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
