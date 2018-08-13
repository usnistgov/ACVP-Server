using NIST.CVP.Common.Oracle.DispositionTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet()
            {
                Algorithm = "ECDSA",
                Mode = "SigVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    Curve = Curve.P192,
                    KeyPair = new EccKeyPair(new EccPoint(1, 2), 3)
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Signature = new EccSignature(1, 2),
                        TestPassed = true,
                        Reason = EcdsaSignatureDisposition.None,
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }

            }

            return vectorSet;
        }
    }
}
