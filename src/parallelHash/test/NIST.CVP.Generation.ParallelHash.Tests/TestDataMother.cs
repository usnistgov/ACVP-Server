using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash;
using NIST.CVP.Crypto.Common.Hash.ParallelHash;
using NIST.CVP.Crypto.ParallelHash;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ParallelHash.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string mode = "", string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "ParallelHash",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Function = mode,
                    DigestSize = groupIdx + 1,
                    BitOrientedInput = true,
                    IncludeNull = true,
                    BitOrientedOutput = true,
                    OutputLength = new MathDomain().AddSegment(new ValueDomainSegment(128)),
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
                        Customization = "custom",
                        CustomizationHex = new BitString(8),
                        Deferred = true,
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("mct", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.ResultsArray = new List<AlgoArrayResponseWithCustomization>
                        {
                            new AlgoArrayResponseWithCustomization
                            {
                                Message = new BitString("123456"),
                                Digest = new BitString("987654"),
                                Customization = "custom"
                            }
                        };
                    }
                }
            }

            return tvs;
        }
    }
}
