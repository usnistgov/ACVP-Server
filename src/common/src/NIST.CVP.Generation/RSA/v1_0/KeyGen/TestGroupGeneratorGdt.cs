using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class TestGroupGeneratorGdt : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "GDT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var algSpec in parameters.AlgSpecs)
            {
                var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);
                var mode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(algSpec.RandPQ);
                if (mode != PrimeGenModes.B33)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    foreach (var primeTest in capability.PrimeTests)
                    {
                        var testGroup = new TestGroup
                        {
                            PrimeGenMode = mode,
                            Modulo = capability.Modulo,
                            PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(primeTest),
                            PubExp = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode),
                            FixedPubExp = new BitString(parameters.FixedPubExp),
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
