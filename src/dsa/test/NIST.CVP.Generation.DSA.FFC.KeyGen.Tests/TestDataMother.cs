using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, bool isSample)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "KeyGen"
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
                    tg.P = new BitString((BigInteger) 42);
                    tg.Q = new BitString((BigInteger) 55);
                    tg.G = new BitString((BigInteger) 67);
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
                        tc.X = new BitString((BigInteger)77);
                        tc.Y = new BitString((BigInteger)100);
                    }
                }
            }

            return vectorSet;
        }
    }
}
