using System.Collections.Generic;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.SigGen
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool isSample = true)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "EDDSA",
                Mode = "SigGen"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.Ed25519,
                    PreHash = false,
                    KeyPair = new EdKeyPair(new BitString(new BigInteger(14)), new BitString(new BigInteger(3)))
                };
                testGroups.Add(tg);

                if (!isSample)
                {
                    tg.KeyPair = new EdKeyPair(new BitString(new BigInteger(-14)), new BitString(new BigInteger(-3)));
                }

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Signature = new EdSignature(new BitString("BEEF")),
                        TestCaseId = testId,
                        ParentGroup = tg,
                        Context = new BitString("BEEFFACE")
                    };
                    tests.Add(tc);

                    if (!isSample)
                    {
                        tc.Signature = new EdSignature(new BitString("FACE"));
                    }
                }
            }

            return vectorSet;
        }
    }
}
