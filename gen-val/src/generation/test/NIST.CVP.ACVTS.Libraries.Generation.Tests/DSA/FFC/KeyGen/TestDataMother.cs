using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, bool isSample)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "keyGen"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    L = 2048 + groupIdx,
                    N = 224
                };
                testGroups.Add(tg);

                if (isSample)
                {
                    tg.DomainParams.P = 42;
                    tg.DomainParams.Q = 55;
                    tg.DomainParams.G = 67;
                }
                else
                {
                    tg.DomainParams.P = 0;
                    tg.DomainParams.Q = 0;
                    tg.DomainParams.G = 0;
                }

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    var tc = new TestCase
                    {
                        TestCaseId = testId,
                        ParentGroup = tg
                    };
                    tests.Add(tc);

                    if (isSample)
                    {
                        tc.Key.PrivateKeyX = 77;
                        tc.Key.PublicKeyY = 100;
                    }
                    else
                    {
                        tc.Key.PrivateKeyX = 0;
                        tc.Key.PublicKeyY = 0;
                    }
                }
            }

            return vectorSet;
        }
    }
}
