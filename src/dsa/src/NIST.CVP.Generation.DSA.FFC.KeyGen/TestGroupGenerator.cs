using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NLog;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
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

                    DsaDomainParametersResult result = null;
                    try
                    {
                        result = _oracle.GetDsaDomainParameters(param);
                    }
                    catch (Exception ex)
                    {
                        ThisLogger.Error(ex.StackTrace);
                        throw;
                    }

                    domainParams = new FfcDomainParameters(result.P, result.Q, result.G);
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

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
