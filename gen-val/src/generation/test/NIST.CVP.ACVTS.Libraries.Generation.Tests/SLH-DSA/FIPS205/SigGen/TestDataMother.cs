using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLHDSA.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.SLH_DSA.FIPS205.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SLH_DSA.FIPS205.SigGen;

public class TestDataMother
{
    public static TestVectorSet GetTestGroups(int groups = 1)
    {
        var tvs = new TestVectorSet { Algorithm = "SLH-DSA", Mode = "sigGen", Revision = "FIPS205", IsSample = true };
        
        var testGroups = new List<TestGroup>();
        tvs.TestGroups = testGroups;
        for (var groupIdx = 0; groupIdx < groups; groupIdx++)
        {
            var tg = new TestGroup
            {
                TestType = "AFT",
                ParameterSet = SlhdsaParameterSet.SLH_DSA_SHA2_128f,
                Deterministic = true
            };
            testGroups.Add(tg);

            var tests = new List<TestCase>();
            tg.Tests = tests;
            for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
            {
                var tc = new TestCase
                {
                    ParentGroup = tg,
                    TestCaseId = testId,
                    PrivateKey = new BitString("ABCD"),
                    AdditionalRandomness = new BitString("123456"),
                    MessageLength = 5,
                    Message = new BitString("789ABC9ABC"),
                    Signature = new BitString("DEF012") 
                };
                tests.Add(tc);
            }
        }
        
        return tvs;
    }
}
