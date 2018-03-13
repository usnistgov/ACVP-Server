using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.SHA2;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string mode = "sha3", string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "SHA",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Function = ModeValues.SHA2,
                    DigestSize = DigestSizes.d224,
                    BitOriented = true,
                    IncludeNull = true,
                    TestType = testType
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        Message = new BitString("ABCD"),
                        Digest = new BitString("ABCDEF"),
                        Deferred = true,
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.ResultsArray = new List<AlgoArrayResponse>
                        {
                            new AlgoArrayResponse
                            {
                                Message = new BitString("123456"),
                                Digest = new BitString("987654")
                            }
                        };
                    }
                }
            }

            return tvs;
        }
    }
}
