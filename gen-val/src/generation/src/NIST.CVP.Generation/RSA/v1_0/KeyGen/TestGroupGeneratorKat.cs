using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class TestGroupGeneratorKat : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.PubExpMode == PublicExponentModes.Fixed)
            {
                return testGroups;
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                if (algSpec.RandPQ != PrimeGenFips186_4Modes.B33)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    foreach (var primeTest in capability.PrimeTests)
                    {
                        // No KATs available for this size
                        if (capability.Modulo == 4096) continue;
                        
                        var testGroup = new TestGroup
                        {
                            PrimeGenMode = algSpec.RandPQ,
                            Modulo = capability.Modulo,
                            PrimeTest = primeTest,
                            PubExp = parameters.PubExpMode,
                            KeyFormat = parameters.KeyFormat,
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
