using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.SigGen
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var vectorSet = new TestVectorSet
            {
                Algorithm = "DSA",
                Mode = "SigGen"
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
                    DomainParams = new FfcDomainParameters(3, 4, 5),
                    Key = new FfcKeyPair(6, 7),
                };

                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Signature = new FfcSignature(1, 2),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }


            }

            return vectorSet;
        }
    }
}
