using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.Fips186_5.KeyGen
{
    public class TestGroupGeneratorGdt : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "GDT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var algSpec in parameters.AlgSpecs)
            {
                if (algSpec.RandPQ != PrimeGenModes.RandomProbablePrimes) continue;

                foreach (var capability in algSpec.Capabilities)
                {
                    foreach (var primeTest in capability.PrimeTests)
                    {
                        var testGroup = new TestGroup
                        {
                            PrimeGenMode = algSpec.RandPQ,
                            Modulo = capability.Modulo,
                            PrimeTest = primeTest,
                            PubExp = parameters.PubExpMode,
                            FixedPubExp = parameters.FixedPubExp,
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