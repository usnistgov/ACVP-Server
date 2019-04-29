using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.v1_0.SigVer.TestCaseExpectations;
using NLog;

namespace NIST.CVP.Generation.DSA.v1_0.SigVer
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

        private async Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

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

                    FfcDomainParameters domainParams = null;
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
                    
                    var testGroup = new TestGroup
                    {
                        L = l,
                        N = n,
                        HashAlg = hashFunction,
                        DomainParams = domainParams,
                        TestCaseExpectationProvider = new TestCaseExpectationProvider(parameters.IsSample)
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }

        private static ILogger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
