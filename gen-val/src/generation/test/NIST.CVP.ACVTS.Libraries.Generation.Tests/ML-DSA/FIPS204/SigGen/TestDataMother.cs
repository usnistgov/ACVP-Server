using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Generation.ML_DSA.FIPS204.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ML_DSA.FIPS204.SigGen;

public static class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet
        {
            Algorithm = "ML-DSA",
            Mode = "keyGen",
            Revision = "FIPS204",
            IsSample = true
        };

        var testGroups = new List<TestGroup>();
        tvs.TestGroups = testGroups;
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                ParameterSet = DilithiumParameterSet.ML_DSA_44,
                PublicKey = new BitString("098765"),
                PrivateKey = new BitString("ABCDEF"),
                Deterministic = true,
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
                    Message = new BitString("ABCD"),
                    Signature = new BitString("1234"),
                    TestCaseId = testId
                };
                tests.Add(tc);
            }
        }

        return tvs;
    }
}
