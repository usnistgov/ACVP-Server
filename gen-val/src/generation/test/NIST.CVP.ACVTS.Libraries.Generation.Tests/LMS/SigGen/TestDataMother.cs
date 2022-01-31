using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigGen
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool isSample = false)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "LMS",
                Mode = "SigGen"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;

            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    LmsTypes = new List<LmsType>(new LmsType[] { LmsType.LMS_SHA256_M32_H5, LmsType.LMS_SHA256_M32_H10 }),
                    LmotsTypes = new List<LmotsType>(new LmotsType[] { LmotsType.LMOTS_SHA256_N32_W8 })
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;

                for (var testId = 5 * groupIdx + 1; testId <= (groupIdx + 1) * 5; testId++)
                {
                    var tc = new TestCase
                    {
                        TestCaseId = testId,
                        Signature = new BitString("72634947394737438392469749337428763874a0bcd3432fe7546ff87cdefac2"),
                        Seed = new BitString("72634947394737438392469749337428763874a0bcd3432fe7546ff87cdefac2"),
                        RootI = new BitString("763874a0bcd3432fe7546ff87cdefac2"),
                        Message = new BitString("72634947394737438392469749337428763874a0bcd3432fe7546ff87cdefac2"),
                        ResultsArray = new List<AlgoArrayResponse>(
                            new AlgoArrayResponse[]
                            {
                                new AlgoArrayResponse() {
                                    Message = new BitString("763874a0bcd3432fe7546ff87cdefac2"),
                                    Signature = new BitString("72634947394737438392469749337428763874a0bcd3432fe7546ff87cdefac2")
                                }
                            }
                        ),
                        ParentGroup = tg
                    };
                    tests.Add(tc);

                    if (!isSample)
                    {
                        tc.Message = new BitString("beefface");
                    }

                }
            }

            return vectorSet;
        }
    }
}
