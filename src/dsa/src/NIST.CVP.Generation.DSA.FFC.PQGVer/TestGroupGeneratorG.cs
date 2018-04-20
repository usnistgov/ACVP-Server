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
    public class TestGroupGeneratorG : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";
        private IShaFactory _shaFactory = new ShaFactory();

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var gGen in capability.GGen)
                {
                    foreach (var hashAlg in capability.HashAlg)
                    {
                        // Gather hash alg
                        var mapping = AlgorithmSpecificationToDomainMapping.GetMappingFromAlgorithm(hashAlg);
                        var hashFunction = _shaFactory.GetShaInstance(new HashFunction(mapping.shaMode, mapping.shaDigestSize)).HashFunction;

                        var testGroup = new TestGroup
                        {
                            GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(gGen),
                            L = capability.L,
                            N = capability.N,
                            HashAlg = hashFunction,
                            GTestCaseExpectationProvider = new GTestCaseExpectationProvider(parameters.IsSample),

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