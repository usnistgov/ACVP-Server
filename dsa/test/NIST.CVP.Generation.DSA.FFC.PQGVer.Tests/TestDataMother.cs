using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, GeneratorGenMode gGenMode, PrimeGenMode pqGenMode)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "PQGVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                TestGroup tg = new TestGroup
                {
                    GGenMode = gGenMode,
                    PQGenMode = pqGenMode,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    L = 2048,
                    N = 256,
                    TestType = "gdt"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for(var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    tests.Add(new TestCase
                    {
                        P = 1,
                        Q = 2,
                        G = 3,
                        H = 4,
                        Reason = "none",
                        TestPassed = true,
                        Seed = new DomainSeed(4, 15, 25),
                        Counter = new Counter(5, 88),
                        Index = new BitString("ABCD"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
