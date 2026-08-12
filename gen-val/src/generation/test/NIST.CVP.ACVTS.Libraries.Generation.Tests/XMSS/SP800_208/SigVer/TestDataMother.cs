using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.XMSS.SP800_208.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.XMSS.SP800_208.SigVer;

public static class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet
        {
            Algorithm = "XMSS",
            Mode = "sigVer",
            Revision = "SP800-208",
            IsSample = true
        };

        var testGroups = new List<TestGroup>();
        tvs.TestGroups = testGroups;
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                TestGroupId = groupIdx + 1,
                XmssMode = XmssMode.XMSS_SHA2_10_256,
                PublicKey = new BitString("0123456789ABCDEF"),
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
                    MessageLength = 32,
                    Message = new BitString("AABBCCDD"),
                    Signature = new BitString("FFEEDDCC"),
                    TestCaseId = testId
                };
                tests.Add(tc);
            }
        }

        return tvs;
    }
}
