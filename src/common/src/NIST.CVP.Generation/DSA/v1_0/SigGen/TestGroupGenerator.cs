using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.v1_0.SigGen
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
            groups.Wait();

            return groups.Result;
        }

        private async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (!parameters.IsSample)
            {
                foreach (var capability in parameters.Capabilities)
                {
                    var n = capability.N;
                    var l = capability.L;

                    foreach (var hashAlg in capability.HashAlg)
                    {
                        var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);

                        var testGroup = new TestGroup
                        {
                            L = l,
                            N = n,
                            HashAlg = hashFunction
                        };

                        testGroups.Add(testGroup);
                    }
                }

                return testGroups;
            }

            // For a sample, we need to generate everything up front (since the IUT would normally do this work)
            Dictionary<TestGroup, Task<DsaDomainParametersResult>> mapDomainParam =
                new Dictionary<TestGroup, Task<DsaDomainParametersResult>>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                foreach (var hashAlg in capability.HashAlg)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);

                    var testGroup = new TestGroup
                    {
                        L = l,
                        N = n,
                        HashAlg = hashFunction
                    };

                    var param = new DsaDomainParametersParameters
                    {
                        HashAlg = hashFunction,
                        PQGenMode = PrimeGenMode.Provable,
                        GGenMode = GeneratorGenMode.Unverifiable,
                        L = l,
                        N = n
                    };

                    mapDomainParam.Add(testGroup, _oracle.GetDsaDomainParametersAsync(param));
                }
            }

            await Task.WhenAll(mapDomainParam.Values);

            Dictionary<TestGroup, Task<DsaKeyResult>> mapKey =
                new Dictionary<TestGroup, Task<DsaKeyResult>>();
            foreach (var keyValuePair in mapDomainParam)
            {
                var group = keyValuePair.Key;
                var domainParam = keyValuePair.Value.Result;
                group.DomainParams = new FfcDomainParameters()
                {
                    G = domainParam.G,
                    P = domainParam.P,
                    Q = domainParam.Q
                };

                mapKey.Add(group, _oracle.GetDsaKeyAsync(new DsaKeyParameters()
                {
                    DomainParameters = group.DomainParams
                }));
            }

            await Task.WhenAll(mapKey.Values);

            foreach (var keyValuePair in mapKey)
            {
                var group = keyValuePair.Key;
                var key = keyValuePair.Value.Result;
                group.Key = key.Key;

                testGroups.Add(group);
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
