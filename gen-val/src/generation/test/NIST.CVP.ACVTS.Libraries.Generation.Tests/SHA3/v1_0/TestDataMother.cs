using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v1_0
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, ModeValues mode = ModeValues.SHA3, string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "SHA3",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Function = mode,
                    DigestSize = DigestSizes.d256,
                    BitOrientedInput = true,
                    IncludeNull = true,
                    BitOrientedOutput = true,
                    OutputLength = new MathDomain().AddSegment(new ValueDomainSegment(groupIdx + 1)),
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
                        DigestLength = 24,
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
