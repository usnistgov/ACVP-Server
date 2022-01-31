using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.SigGen
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

            // If modulo size 2048 exists anywhere in the capabilities
            if (parameters.Capabilities.Any(x => x.ModuloCapabilities.Any(y => y.Modulo == 2048)))
            {
                list.Add(new TestGroupGeneratorHighAssuranceCryptoTest(_oracle));
            }

            return list;
        }
    }
}
