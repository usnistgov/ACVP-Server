using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Key = new KeyPair
                        {
                            PubKey = new PublicKey{E = 1, N = 2},
                            PrivKey = new PrivateKey{D = 3, P = 4, Q = 5}
                        },
                        Seed = new BitString("BEEFFACE"),
                        Bitlens = new[] {1,2,3,4},
                        XP = new BitString("01"),
                        XP1 = new BitString("02"),
                        XP2 = new BitString("03"),
                        XQ = new BitString("04"),
                        XQ1 = new BitString("05"),
                        XQ2 = new BitString("06"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                    InfoGeneratedByServer = true,
                    PrimeGenMode = PrimeGenModes.B35,
                    Modulo = 1 + groupIdx,
                    TestType = "aft",
                    PubExp = PublicExponentModes.Random,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
