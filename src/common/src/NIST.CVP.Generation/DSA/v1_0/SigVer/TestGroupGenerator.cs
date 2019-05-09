using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.v1_0.SigVer.TestCaseExpectations;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly bool _randomizeMessagePriorToSign;

        public TestGroupGenerator(IOracle oracle, bool randomizeMessagePriorToSign)
        {
            _oracle = oracle;
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var groups = BuildTestGroupsAsync(parameters);
            groups.Wait();

            return groups.Result;
        }

        private async Task<HashSet<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            Dictionary<TestGroup, Task<DsaDomainParametersResult>> map = new Dictionary<TestGroup, Task<DsaDomainParametersResult>>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                foreach (var hashAlg in capability.HashAlg)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);
                    var param = new DsaDomainParametersParameters
                    {
                        HashAlg = hashFunction,
                        PQGenMode = PrimeGenMode.Provable,
                        GGenMode = GeneratorGenMode.Unverifiable,
                        L = l,
                        N = n
                    };

                    var testGroup = new TestGroup
                    {
                        L = l,
                        N = n,
                        HashAlg = hashFunction,
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample),
                        IsMessageRandomized = _randomizeMessagePriorToSign
                    };

                    map.Add(testGroup, _oracle.GetDsaDomainParametersAsync(param));
                }
            }

            await Task.WhenAll(map.Values);

            HashSet<TestGroup> groups = new HashSet<TestGroup>();
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

                groups.Add(group);
            }

            return groups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
