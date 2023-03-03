using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.KeyGen
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool isSample = false)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "ECDSA",
                Mode = "keyGen"
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

                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
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
                        tc.KeyPair = new EccKeyPair(new EccPoint(0, 0), 0);
                    }

                }
            }

            return vectorSet;
        }
    }
}
