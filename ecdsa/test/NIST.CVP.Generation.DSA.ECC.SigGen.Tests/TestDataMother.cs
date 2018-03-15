using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, bool isSample = true)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "ECDSA",
                Mode = "SigGen"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Curve = Curve.P192,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    KeyPair = new EccKeyPair(new EccPoint(1, 2), 3)
                };
                testGroups.Add(tg);

                if (!isSample)
                {
                    tg.KeyPair = new EccKeyPair(new EccPoint(-1, -2), -3);
                }

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Signature = new EccSignature(1, 2),
                        TestCaseId = testId,
                        ParentGroup = tg
                    };
                    tests.Add(tc);

                    if (!isSample)
                    {
                        tc.Signature = new EccSignature(-1, -2);
                    }
                }
            }

            return vectorSet;
        }
    }
}
