using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.Ed;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.Ed.SigGen.Tests
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
                    KeyPair = new EdKeyPair(new BitString("ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf").ToPositiveBigInteger(), 14)
                };
                testGroups.Add(tg);

                if (!isSample)
                {
                    tg.KeyPair = new EdKeyPair(new BitString("3d4017c3e843895a92b70aa74d1b7ebc9c982ccf2ec4968cc0cd55f12af4660c").ToPositiveBigInteger(), 14);
                }

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Signature = new EdSignature(5),
                        TestCaseId = testId,
                        ParentGroup = tg,
                        Context = new BitString("BEEFFACE")
                    };
                    tests.Add(tc);

                    if (!isSample)
                    {
                        tc.Signature = new EdSignature(-4);
                    }
                }
            }

            return vectorSet;
        }
    }
}
