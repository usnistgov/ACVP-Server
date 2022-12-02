using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CCM.v1_0;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent
{
    public class TestGroupGeneratorFactory : ITestGroupGeneratorFactory<Parameters, TestGroup, TestCase>
    {
        public IEnumerable<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>> GetTestGroupGenerators(Parameters parameters)
        {
            var list = new HashSet<ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>>
            {
                new TestGroupGenerator(),
            };

            return list;
        }
    }   
}
