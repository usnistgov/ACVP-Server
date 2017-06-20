using System.Collections.Generic;
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

            foreach (var mode in parameters.KeyGenModes)
            {
                // No AFTs for this mode
                if (mode.ToLower() == "b.3.3")
                {
                    continue;
                }

                foreach (var modulo in parameters.Moduli)
                {
                    // All provable
                    if (mode.ToLower() == "b.3.2" || mode.ToLower() == "b.3.4")
                    {
                        foreach (var hashAlg in parameters.HashAlgs)
                        {
                            var testGroup = new TestGroup
                            {
                                Mode = RSAEnumHelpers.StringToKeyGenMode(mode),
                                Modulo = modulo,
                                HashAlg = SHAEnumHelpers.StringToHashFunction(hashAlg),
                                PubExp = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode),
                                FixedPubExp = new BitString(parameters.FixedPubExp),
                                InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                TestType = TEST_TYPE
                            };

                            testGroups.Add(testGroup);
                        }
                    }

                    // Both probable and provable
                    if (mode.ToLower() == "b.3.5")
                    {
                        foreach (var hashAlg in parameters.HashAlgs)
                        {
                            foreach (var primeTest in parameters.PrimeTests)
                            {
                                var testGroup = new TestGroup
                                {
                                    Mode = RSAEnumHelpers.StringToKeyGenMode(mode),
                                    Modulo = modulo,
                                    PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest),
                                    HashAlg = SHAEnumHelpers.StringToHashFunction(hashAlg),
                                    PubExp = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode),
                                    FixedPubExp = new BitString(parameters.FixedPubExp),
                                    InfoGeneratedByServer = parameters.InfoGeneratedByServer,
                                    TestType = TEST_TYPE
                                };

                                testGroups.Add(testGroup);
                            }
                        }
                    }

                    // All probable
                    if (mode.ToLower() == "b.3.6")
                    {
                        foreach (var primeTest in parameters.PrimeTests)
                        {
                            var testGroup = new TestGroup
                            {
                                Mode = RSAEnumHelpers.StringToKeyGenMode(mode),
                                Modulo = modulo,
                                PrimeTest = RSAEnumHelpers.StringToPrimeTestMode(primeTest),
                                PubExp = RSAEnumHelpers.StringToPubExpMode(parameters.PubExpMode),
                                FixedPubExp = new BitString(parameters.FixedPubExp),
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
