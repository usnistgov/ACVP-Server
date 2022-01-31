using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigVer
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGeneratorFactory(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator(_oracle, false)
            };

            if (parameters.Conformances.Contains("SP800-106", StringComparer.OrdinalIgnoreCase))
            {
                list.Add(new TestGroupGenerator(_oracle, true));
            }

            return list;
        }
    }
}
