using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Generation.EDDSA.v1_0.KeyGen;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool isSample = false)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "EDDSA",
                Mode = "KeyGen"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;

            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.Ed25519,
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
                        KeyPair = new EdKeyPair(new BitString("BEEF"), new BitString("FACE")),
                        ParentGroup = tg
                    };
                    tests.Add(tc);

                    if (!isSample)
                    {
                        tc.KeyPair = new EdKeyPair(new BitString("FACE"), new BitString("BEEF"));
                    }

                }
            }

            return vectorSet;
        }
    }
}
