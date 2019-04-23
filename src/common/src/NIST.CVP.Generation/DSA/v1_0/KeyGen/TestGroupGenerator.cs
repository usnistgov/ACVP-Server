using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NLog;

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
            groups.Wait();
            return groups.Result;
        }

        public async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                FfcDomainParameters domainParams = null;
                if (parameters.IsSample)
                {
                    var param = new DsaDomainParametersParameters
                    {
                        GGenMode = GeneratorGenMode.Unverifiable,
                        PQGenMode = PrimeGenMode.Provable,
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                        L = l,
                        N = n
                    };

                    try
                    {
                        var result = await _oracle.GetDsaDomainParametersAsync(param);

                        domainParams = new FfcDomainParameters(result.P, result.Q, result.G);
                    }
                    catch (Exception ex)
                    {
                        ThisLogger.Error(ex);
                        throw;
                    }

                    if (domainParams == null)
                    {
                        ThisLogger.Error($"ERROR: Domain Parameters are null for group with properties l={l}, n={n}");
                    }
                }

                var testGroup = new TestGroup
                {
                    L = l,
                    N = n,
                    DomainParams = domainParams
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
