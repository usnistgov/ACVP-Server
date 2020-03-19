using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KDF_Components.v1_0.SNMP
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var engineId in parameters.EngineId)
            {
                foreach (var passwordLen in parameters.PasswordLength.GetDomainMinMaxAsEnumerable())
                {
                    list.Add(new TestGroup
                    {
                        EngineId = new BitString(engineId),
                        PasswordLength = passwordLen
                    });
                }
            }

            return Task.FromResult(list.AsEnumerable());
        }
    }
}
