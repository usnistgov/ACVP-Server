using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGGen
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, GeneratorGenMode gGenMode, PrimeGenMode pqGenMode)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "pqgGen"
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
                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    var tc = new TestCase
                    {
                        P = 1,
                        Q = 2,
                        G = 3,
                        Seed = new DomainSeed(4, 1000, 2000),
                        Counter = new Counter(5, 77)
                        {
                            Count = 89
                        },
                        Index = new BitString("ABCD"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    };

                    tests.Add(tc);
                }
            }

            return vectorSet;
        }
    }
}
