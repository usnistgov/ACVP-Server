using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KDA.Shared.Hkdf;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDA.Sp800_56Cr2.Hkdf
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list =
                new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>()
                {
                    new TestGroupGenerator(),
                };

            return list;
        }
    }
}
