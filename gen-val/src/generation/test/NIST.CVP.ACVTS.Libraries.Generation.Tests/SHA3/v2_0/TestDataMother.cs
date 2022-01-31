using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Libraries.Math.LargeBitString;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v2_0
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "SHA3-224",
                Revision = "2.0",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    HashFunction = new HashFunction(ModeValues.SHA3, DigestSizes.d224),
                    LargeDataSizes = new[] { 1, 2 },
                    MessageLengths = new MathDomain().AddSegment(new ValueDomainSegment(groupIdx + 1)),
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
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("aft", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.Digest = new BitString("ABCDEF");
                    }
                    else if (testType.Equals("mct", StringComparison.OrdinalIgnoreCase))
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
                    else if (testType.Equals("ldt", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.LargeMessage = new LargeBitString { Content = new BitString("ABCD"), ExpansionTechnique = ExpansionMode.Repeating, FullLength = 48 };
                        tc.Digest = new BitString("BEEFFACE");
                    }
                }
            }

            return tvs;
        }
    }
}
