using System.Collections.Generic;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

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
                var mode = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(algSpec.RandPQ);
                var pubExpMode = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(parameters.PubExpMode);
                var keyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(parameters.KeyFormat);

                BitString pubExpVal = null;
                if (pubExpMode == PublicExponentModes.Fixed)
                {
                    pubExpVal = new BitString(parameters.FixedPubExp);
                }

                if (mode == PrimeGenModes.B33)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    // All provable
                    if (mode == PrimeGenModes.B32 || mode == PrimeGenModes.B34)
                    {
                        foreach (var hashAlg in capability.HashAlgs)
                        {
                            var testGroup = new TestGroup
                            {
                                PrimeGenMode = mode,
                                Modulo = capability.Modulo,
                                HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                                PubExp = pubExpMode,
                                FixedPubExp = pubExpVal,
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                KeyFormat = keyFormat,
                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }

                    // Both probable and provable
                    if (mode == PrimeGenModes.B35)
                    {
                        foreach (var hashAlg in capability.HashAlgs)
                        {
                            foreach (var primeTest in capability.PrimeTests)
                            {
                                var testGroup = new TestGroup
                                {
                                    PrimeGenMode = mode,
                                    Modulo = capability.Modulo,
                                    PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(primeTest),
                                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                                    PubExp = pubExpMode,
                                    FixedPubExp = pubExpVal,
                                    InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                    KeyFormat = keyFormat,
                                    TestType = TEST_TYPE
                                };

                                testGroups.Add(testGroup);
                            }
                        }
                    }

                    // All probable
                    if (mode == PrimeGenModes.B36)
                    {
                        foreach (var primeTest in capability.PrimeTests)
                        {
                            var testGroup = new TestGroup
                            {
                                PrimeGenMode = mode,
                                Modulo = capability.Modulo,
                                PrimeTest = EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(primeTest),
                                PubExp = pubExpMode,
                                FixedPubExp = pubExpVal,
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                KeyFormat = keyFormat,
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
