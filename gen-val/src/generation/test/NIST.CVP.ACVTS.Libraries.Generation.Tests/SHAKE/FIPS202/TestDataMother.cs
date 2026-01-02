using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHAKE.FIPS202
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "SHAKE-128",
                Revision = "2.0",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    HashFunction = new HashFunction(ModeValues.SHAKE, DigestSizes.d128),
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
                    tc.Digest = new BitString("ABCDEF");
                }
            }

            return tvs;
        }
    }
}
