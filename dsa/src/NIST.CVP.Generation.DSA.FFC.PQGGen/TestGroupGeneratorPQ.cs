using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestGroupGeneratorPQ : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";
        public const string TEST_MODE = "PQ";
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
                            PQGenMode = EnumHelper.StringToPQGenMode(pqGen),
                            L = capability.L,
                            N = capability.N,
                            HashAlg = hashFunction,

                            TestType = TEST_TYPE,
                            TestMode = TEST_MODE
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
