using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NLog;

namespace NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigVer
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private readonly IOracle _oracle;
        private readonly bool _randomizeMessagePriorToSign;

        public TestGroupGenerator(IOracle oracle, bool randomizeMessagePriorToSign)
        {
            _oracle = oracle;
            _randomizeMessagePriorToSign = randomizeMessagePriorToSign;
        }

        public async Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
                        Conformance = _randomizeMessagePriorToSign ? "SP800-106" : null
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

            return groups.ToList();
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
