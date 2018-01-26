using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestGroupGeneratorAlgorithmFunctionalTest : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "AFT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var algSpec in parameters.AlgSpecs)
            {
                var mode = RSAEnumHelpers.StringToKeyGenMode(algSpec.RandPQ);
                var pubExpMode = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode);

                BitString pubExpVal = new BitString(0);
                if (pubExpMode == PubExpModes.FIXED)
                {
                    pubExpVal = new BitString(parameters.FixedPubExp);
                }

                if (mode == KeyGenModes.B33)
                {
                    continue;
                }

                foreach (var capability in algSpec.Capabilities)
                {
                    // All provable
                    if (mode == KeyGenModes.B32 || mode == KeyGenModes.B34)
                    {
                        foreach (var hashAlg in capability.HashAlgs)
                        {
                            var testGroup = new TestGroup
                            {
                                Mode = mode,
                                Modulo = capability.Modulo,
                                HashAlg = SHAEnumHelpers.StringToHashFunction(hashAlg),
                                PubExp = pubExpMode,
                                FixedPubExp = pubExpVal,
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }

                    // Both probable and provable
                    if (mode == KeyGenModes.B35)
                    {
                        foreach (var hashAlg in capability.HashAlgs)
                        {
                            foreach (var primeTest in capability.PrimeTests)
                            {
                                var testGroup = new TestGroup
                                {
                                    Mode = mode,
                                    Modulo = capability.Modulo,
                                    PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest),
                                    HashAlg = SHAEnumHelpers.StringToHashFunction(hashAlg),
                                    PubExp = pubExpMode,
                                    FixedPubExp = pubExpVal,
                                    InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                    TestType = TEST_TYPE
                                };

                                testGroups.Add(testGroup);
                            }
                        }
                    }

                    // All probable
                    if (mode == KeyGenModes.B36)
                    {
                        foreach (var primeTest in capability.PrimeTests)
                        {
                            var testGroup = new TestGroup
                            {
                                Mode = mode,
                                Modulo = capability.Modulo,
                                PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest),
                                PubExp = pubExpMode,
                                FixedPubExp = pubExpVal,
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
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
