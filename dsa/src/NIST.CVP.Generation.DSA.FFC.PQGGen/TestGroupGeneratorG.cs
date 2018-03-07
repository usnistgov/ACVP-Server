using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen
{
    public class TestGroupGeneratorG : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "GDT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var gGen in capability.GGen)
                {
                    foreach (var hashAlg in capability.HashAlgs)
                    {
                        var testGroup = new TestGroup
                        {
                            GGenMode = EnumHelpers.GetEnumFromEnumDescription<GeneratorGenMode>(gGen),
                            PQGenMode = PrimeGenMode.Probable,
                            L = capability.L,
                            N = capability.N,
                            HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),

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