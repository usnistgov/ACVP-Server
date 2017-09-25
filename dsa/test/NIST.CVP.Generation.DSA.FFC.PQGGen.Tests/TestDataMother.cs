using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.FFC.PQGGen.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(string mode = "pq", int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for(var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    tests.Add(new TestCase
                    {
                        P = 1,
                        Q = 2,
                        G = 3,
                        Seed = new DomainSeed(4),
                        Counter = new Counter(5),
                        Index = new BitString("ABCD"),
                        TestCaseId = testId
                    });
                }

                if (mode == "pq")
                {
                    testGroups.Add(new TestGroup
                    {
                        GGenMode = GeneratorGenMode.None,
                        PQGenMode = PrimeGenMode.Probable,
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                        L = 2048,
                        N = 256,
                        TestType = "gdt",
                        Tests = tests
                    });
                }
                else if(mode == "g")
                {
                    testGroups.Add(new TestGroup
                    {
                        GGenMode = GeneratorGenMode.Canonical,
                        PQGenMode = PrimeGenMode.None,
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                        L = 2048,
                        N = 256,
                        TestType = "gdt",
                        Tests = tests
                    });
                }
                
            }

            return testGroups;
        }
    }
}
