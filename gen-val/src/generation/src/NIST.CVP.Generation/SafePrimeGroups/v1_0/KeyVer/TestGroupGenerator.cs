using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SafePrimeGroups.v1_0.KeyVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }
        
        public async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var safePrimeGroup in parameters.SafePrimeGroups)
            {
                var testGroup = new TestGroup
                {
                    SafePrimeGroup = safePrimeGroup
                };

                testGroups.Add(testGroup);
            }

            await GetDomainParametersForGroups(testGroups);
            
            return testGroups;
        }

        private async Task GetDomainParametersForGroups(List<TestGroup> testGroups)
        {
            var tasks = new Dictionary<TestGroup, Task<FfcDomainParameters>>();
            foreach (var group in testGroups)
            {
                tasks.Add(group, _oracle.GetSafePrimeGroupsDomainParameterAsync(group.SafePrimeGroup));
            }

            await Task.WhenAll(tasks.Values);

            foreach (var (group, value) in tasks)
            {
                group.DomainParameters = value.Result;
            }
        }
    }
}