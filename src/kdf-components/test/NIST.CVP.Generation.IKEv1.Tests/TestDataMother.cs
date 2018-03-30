using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Math;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.IKEv1.Enums;

namespace NIST.CVP.Generation.IKEv1.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string authMode = "dsa")
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
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
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        AuthenticationMethod = EnumHelpers.GetEnumFromEnumDescription<AuthenticationMethods>(authMode),
                        HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                        PreSharedKeyLength = groupIdx + 64,
                        NInitLength = groupIdx + 64,
                        NRespLength = groupIdx + 64,
                        GxyLength = groupIdx + 64,
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }
            return testGroups;
        }
    }
}
