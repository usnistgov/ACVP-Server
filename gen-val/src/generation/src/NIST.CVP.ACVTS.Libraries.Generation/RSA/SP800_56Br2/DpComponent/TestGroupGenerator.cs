using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.RSA.Sp800_56Br2.DpComponent
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            List<TestGroup> groups = new List<TestGroup>();

            foreach (var format in parameters.KeyFormat)
            {
                foreach (var mod in parameters.Modulus)
                {
                    groups.Add(new TestGroup
                    {
                        Modulo = mod,
                        KeyMode = format,
                        TestType = TEST_TYPE
                    });
                }
            }

            return Task.FromResult(groups);
        }
    }    
}
