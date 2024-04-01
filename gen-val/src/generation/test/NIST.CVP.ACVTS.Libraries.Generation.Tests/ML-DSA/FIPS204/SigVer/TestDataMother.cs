using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigVer;

public static class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet
        {
            Algorithm = "LMS",
            Mode = "sigVer",
            Revision = "1.0",
            IsSample = true
        };

        var testGroups = new List<TestGroup>();
        tvs.TestGroups = testGroups;
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                ParameterSet = DilithiumParameterSet.ML_DSA_44,
                PrivateKey = new BitString("1234"),
                PublicKey = new BitString("098765"),
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
                    Message = new BitString("ABCDEF"),
                    Signature = new BitString("123456"),
                    TestPassed = true,
                    TestCaseId = testId
                };
                tests.Add(tc);
            }
        }

        return tvs;
    }
}
