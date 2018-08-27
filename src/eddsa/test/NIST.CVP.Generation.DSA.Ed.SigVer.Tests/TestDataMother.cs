using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "EDDSA",
                Mode = "SigVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.Ed25519,
                    PreHash = false,
                    KeyPair = new EdKeyPair(new BitString("BEEF"), new BitString("FACE"))
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
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
