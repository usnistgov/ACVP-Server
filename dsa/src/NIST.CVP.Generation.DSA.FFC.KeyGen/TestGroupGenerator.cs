using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private readonly IDsaFfcFactory _dsaFactory;

        public TestGroupGenerator(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                FfcDomainParameters domainParams = null;
                if (parameters.IsSample)
                {
                    var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
                    var domainParamsRequest = new FfcDomainParametersGenerateRequest(n, l, n, hashFunction.OutputLen, null, PrimeGenMode.Provable, GeneratorGenMode.Unverifiable);
                    var ffcDsa = _dsaFactory.GetInstance(hashFunction);
                    var domainParamsResult = ffcDsa.GenerateDomainParameters(domainParamsRequest);

                    if (!domainParamsResult.Success)
                    {
                        ThisLogger.Error($"Failure generating domain parameters for L = {l}, N = {n}: {domainParamsResult.ErrorMessage}");
                        continue;
                    }

                    domainParams = domainParamsResult.PqgDomainParameters;
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
