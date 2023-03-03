using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SigGen
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "RSA",
                Mode = "sigGen",
                IsSample = true,
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Modulo = 2048,
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                    Mode = SignatureSchemes.Pss,
                    SaltLen = 10,
                    Key = new KeyPair { PubKey = new PublicKey { E = 1, N = 2 } },
                    TestType = "aft"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        Salt = new BitString("123456"),
                        ParentGroup = tg,
                        Message = new BitString("ABCD"),
                        Signature = new BitString("1234"),
                        Deferred = true,
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }
    }
}
