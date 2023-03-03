using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.SigVer
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "EDDSA",
                Mode = "sigVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.Ed25519,
                    PreHash = false
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        KeyPair = new EdKeyPair(new BitString("BEEF"), new BitString("FACE")),
                        Message = new BitString("BEEFFACE"),
                        Context = new BitString("BEEFFACE"),
                        Signature = new EdSignature(new BitString("BEEF")),
                        TestPassed = true,
                        Reason = EddsaSignatureDisposition.None,
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }

            }

            return vectorSet;
        }
    }
}
