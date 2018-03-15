using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool isSample = false)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "ECDSA",
                Mode = "KeyGen"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;

            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.B571,
                    SecretGenerationMode = SecretGenerationMode.TestingCandidates
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;

                for(var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    var tc = new TestCase
                    {
                        TestCaseId = testId,
                        KeyPair = new EccKeyPair(new EccPoint(1, 2), 3),
                        ParentGroup = tg
                    };
                    tests.Add(tc);

                    if (!isSample)
                    {
                        tc.KeyPair = new EccKeyPair(new EccPoint(-1, -2), -3);
                    }

                }
            }

            return vectorSet;
        }
    }
}
