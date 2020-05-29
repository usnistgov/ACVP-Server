using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.v1_0.PqgGen
{
    public class TestGroupGeneratorPQ : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var pqGen in capability.PQGen)
                {
                    foreach (var hashAlg in capability.HashAlg)
                    {
                        var testGroup = new TestGroup
                        {
                            PQGenMode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenMode>(pqGen),
                            GGenMode = GeneratorGenMode.None,
                            L = capability.L,
                            N = capability.N,
                            HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),

                            TestType = TEST_TYPE,
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
