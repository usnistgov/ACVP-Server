using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public class TestGroupGeneratorAft : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var algSpec in parameters.AlgSpecs)
            {
                if (algSpec.RandPQ == PrimeGenModes.RandomProbablePrimes)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    // All provable
                    if (algSpec.RandPQ == PrimeGenModes.RandomProvablePrimes || algSpec.RandPQ == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes)
                    {
                        foreach (var hashAlg in capability.HashAlgs)
                        {
                            var testGroup = new TestGroup
                            {
                                PrimeGenMode = algSpec.RandPQ,
                                Modulo = capability.Modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                                PubExp = parameters.PubExpMode,
                                FixedPubExp = parameters.FixedPubExp,
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                KeyFormat = parameters.KeyFormat,
                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }

                    // Both probable and provable
                    if (algSpec.RandPQ == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes)
                    {
                        foreach (var hashAlg in capability.HashAlgs)
                        {
                            foreach (var primeTest in capability.PrimeTests)
                            {
                                var testGroup = new TestGroup
                                {
                                    PrimeGenMode = algSpec.RandPQ,
                                    Modulo = capability.Modulo,
                                    PrimeTest = primeTest,
                                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                                    PubExp = parameters.PubExpMode,
                                    FixedPubExp = parameters.FixedPubExp,
                                    InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                    KeyFormat = parameters.KeyFormat,
                                    TestType = TEST_TYPE
                                };

                                testGroups.Add(testGroup);
                            }
                        }
                    }

                    // All probable
                    if (algSpec.RandPQ == PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes)
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
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                KeyFormat = parameters.KeyFormat,
                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }
                }
            }

            return testGroups;
        }
    }
}
