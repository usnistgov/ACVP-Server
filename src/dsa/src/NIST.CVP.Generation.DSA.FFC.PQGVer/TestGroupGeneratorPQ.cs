using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.PQGVer.TestCaseExpectations;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestGroupGeneratorPQ : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var pqGen in capability.PQGen)
                {
                    foreach (var hashAlg in capability.HashAlgs)
                    {
                        var testGroup = new TestGroup
                        {
                            PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(pqGen),
                            GGenMode = GeneratorGenMode.None,
                            L = capability.L,
                            N = capability.N,
                            HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                            PQTestCaseExpectationProvider = new PQTestCaseExpectationProvider(parameters.IsSample),

                            TestType = TEST_TYPE
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
