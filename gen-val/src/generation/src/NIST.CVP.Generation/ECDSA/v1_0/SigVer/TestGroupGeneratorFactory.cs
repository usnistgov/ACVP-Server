using NIST.CVP.Generation.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NIST.CVP.Generation.ECDSA.v1_0.SigVer
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator(false)
            };

            if (parameters.Conformances.Contains("SP800-106", StringComparer.OrdinalIgnoreCase))
            {
                list.Add(new TestGroupGenerator(true));
            }

            return list;
        }
    }
}
