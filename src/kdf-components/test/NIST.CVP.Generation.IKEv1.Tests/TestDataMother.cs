using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Math;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;
using NIST.CVP.Generation.KDF_Components.v1_0.IKEv1;

namespace NIST.CVP.Generation.IKEv1.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, AuthenticationMethods authMode = AuthenticationMethods.Dsa)
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;

            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    AuthenticationMethod = authMode,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    PreSharedKeyLength = groupIdx + 64,
                    NInitLength = groupIdx + 64,
                    NRespLength = groupIdx + 64,
                    GxyLength = groupIdx + 64,
                    TestType = "Sample"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        CkyInit = new BitString("1AAADFF1"),
                        CkyResp = new BitString("1AAADFF0"),
                        NResp = new BitString("1AAADFFA"),
                        NInit = new BitString("1AAADFFB"),
                        Gxy = new BitString("1AAADFFC"),
                        PreSharedKey = new BitString("1AAADFFD"),
                        SKeyId = new BitString("1AAADFFE"),
                        SKeyIdD = new BitString("7EADDCAB"),
                        SKeyIdA = new BitString("7EADDC"),
                        SKeyIdE = new BitString("9998ADCD"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    });
                }
            }
            return vectorSet;
        }
    }
}
