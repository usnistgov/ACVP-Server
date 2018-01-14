using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestGroupGeneratorPQ : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";
        private IShaFactory _shaFactory = new ShaFactory();

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var pqGen in capability.PQGen)
                {
                    foreach (var hashAlg in capability.HashAlgs)
                    {
                        // Gather hash alg
                        var mapping = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(hashAlg);
                        var hashFunction = _shaFactory.GetShaInstance(new HashFunction(mapping.shaMode, mapping.shaDigestSize)).HashFunction;

                        var testGroup = new TestGroup
                        {
                            PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(pqGen),
                            L = capability.L,
                            N = capability.N,
                            HashAlg = hashFunction,
                            PQTestCaseExpectationProvider = new PQTestCaseExpectationProvider(parameters.IsSample),

                            TestType = TEST_TYPE,
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
