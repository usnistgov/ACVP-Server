using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGenerator<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGenerator<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGeneratorAft(),
                new TestGroupGeneratorKat(),
                new TestGroupGeneratorGdt()
            };

            return list;
        }
    }
}