using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.v1_0.KeyGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;

        public TestGroupGenerator(IOracle oracle)
        {
            _oracle = oracle;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = BuildTestGroupsAsync(parameters);
            return groups.Result.ToArray();
        }

        private async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (!parameters.IsSample)
            {
                foreach (var capability in parameters.Capabilities)
                {
                    var n = capability.N;
                    var l = capability.L;

                    testGroups.Add(new TestGroup
                    {
                        L = l,
                        N = n
                    });
                }

                return testGroups;
            }

            // For a sample, we need to generate domain parameters up front
            Dictionary<TestGroup, Task<DsaDomainParametersResult>> map = new Dictionary<TestGroup, Task<DsaDomainParametersResult>>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                var testGroup = new TestGroup
                {
                    L = l,
                    N = n
                };

                var param = new DsaDomainParametersParameters
                {
                    GGenMode = GeneratorGenMode.Unverifiable,
                    PQGenMode = PrimeGenMode.Provable,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    L = l,
                    N = n
                };

                map.Add(testGroup, _oracle.GetDsaDomainParametersAsync(param));
            }

            await Task.WhenAll(map.Values);

            foreach (var keyValuePair in map)
            {
                var group = keyValuePair.Key;
                var domainParam = keyValuePair.Value.Result;
                group.DomainParams = new FfcDomainParameters()
                {
                    G = domainParam.G,
                    P = domainParam.P,
                    Q = domainParam.Q
                };

                testGroups.Add(group);
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
