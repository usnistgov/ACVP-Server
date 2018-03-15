using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.DSA.FFC.SigVer.FailureHandlers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;

namespace NIST.CVP.Generation.DSA.FFC.SigVer.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "SigVer"
            };

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    L = 2048 + groupIdx,
                    N = 224,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    DomainParams = new FfcDomainParameters(1, 2, 3)
                };

                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Key = new FfcKeyPair(1, 2),
                        Message = new BitString("BEEFFACE"),
                        Signature = new FfcSignature(1, 2),
                        TestPassed = true,
                        Reason = new TestCaseExpectationReason(SigFailureReasons.None),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
