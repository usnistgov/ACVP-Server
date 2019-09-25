using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class TestGroupGeneratorKat : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);
            var pubExpMode = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode);
            if (pubExpMode == PublicExponentModes.Fixed)
            {
                return testGroups;
            }

            foreach (var algSpec in parameters.AlgSpecs)
            {
                var mode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(algSpec.RandPQ);
                if (mode != PrimeGenModes.B33)
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
                            PrimeGenMode = mode,
                            Modulo = capability.Modulo,
                            PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(primeTest),
                            PubExp = pubExpMode,
                            KeyFormat = keyFormat,
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
