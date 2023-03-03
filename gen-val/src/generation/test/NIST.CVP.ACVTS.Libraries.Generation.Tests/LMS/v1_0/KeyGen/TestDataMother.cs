using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.v1_0.KeyGen;

public static class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet
        {
            Algorithm = "LMS",
            Mode = "keyGen",
            Revision = "1.0",
            IsSample = true
        };

        var testGroups = new List<TestGroup>();
        tvs.TestGroups = testGroups;
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                LmsMode = LmsMode.LMS_SHA256_M24_H5,
                LmOtsMode = LmOtsMode.LMOTS_SHA256_N24_W1,
                TestType = "AFT"
            };
            testGroups.Add(tg);

            var tests = new List<TestCase>();
            tg.Tests = tests;
            for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
            {
                var tc = new TestCase
                {
                    ParentGroup = tg,
                    I = new BitString("ABCDEF"),
                    Seed = new BitString("123456"),
                    PublicKey = new BitString("098765"),
                    TestCaseId = testId
                };
                tests.Add(tc);
            }
        }

        return tvs;
    }
}
