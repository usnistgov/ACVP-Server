using System.Collections.Generic;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3.Ecc
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
            var list =
                new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGenerator(_oracle),
                };

            return list;
        }
    }
}