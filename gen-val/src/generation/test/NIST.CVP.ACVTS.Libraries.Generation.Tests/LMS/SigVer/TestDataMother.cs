using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigVer
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "LMS",
                Mode = "SigVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    LmsTypes = new List<LmsType>(new LmsType[] { LmsType.LMS_SHA256_M32_H5 }),
                    LmotsTypes = new List<LmotsType>(new LmotsType[] { LmotsType.LMOTS_SHA256_N32_W4 })
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Signature = new BitString("BEEEEEEEEFFACE"),
                        PublicKey = new BitString("FACEBEEF"),
                        TestPassed = true,
                        Reason = LmsSignatureDisposition.None,
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }

            }

            return vectorSet;
        }
    }
}
