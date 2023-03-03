using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigVer.TestCaseExpectations;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.DispositionTypes;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.SigVer
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "sigVer"
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
                    DomainParams = new FfcDomainParameters(4, 5, 6)
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
                        Reason = new TestCaseExpectationReason(DsaSignatureDisposition.None),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }

            return vectorSet;
        }
    }
}
