using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly IDsaFfcFactory _dsaFactory;

        public TestGroupGenerator(IDsaFfcFactory dsaFactory)
        {
            _dsaFactory = dsaFactory;
        }

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                var n = capability.N;
                var l = capability.L;

                foreach (var hashAlg in capability.HashAlg)
                {
                    var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);
                    var ffcDsa = _dsaFactory.GetInstance(hashFunction);

                    // IUT normally generates PQG
                    FfcDomainParameters domainParams = null;
                    if (parameters.IsSample)
                    {
                        var domainParamsRequest = new FfcDomainParametersGenerateRequest(n, l, n, 256, null, PrimeGenMode.Provable, GeneratorGenMode.Unverifiable);
                        var domainParamsResult = ffcDsa.GenerateDomainParameters(domainParamsRequest);
                        domainParams = domainParamsResult.PqgDomainParameters;
                    }

                    var testGroup = new TestGroup
                    {
                        L = l,
                        N = n,
                        HashAlg = hashFunction,
                        DomainParams = domainParams
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
