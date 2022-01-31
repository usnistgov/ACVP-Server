using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Fips186_5.KeyGen
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGeneratorAft(),
                new TestGroupGeneratorKat(),
                new TestGroupGeneratorGdt()
            };

            return list;
        }
    }
}
