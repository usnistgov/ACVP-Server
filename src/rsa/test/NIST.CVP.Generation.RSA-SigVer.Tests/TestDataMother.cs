using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.RSA_SigVer.TestCaseExpectations;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    public class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1)
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "RSA",
                Mode = "SigVer",
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
                    Key = new KeyPair {PubKey = new PublicKey {E = 1, N = 2}},
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
                        Reason = new TestCaseExpectationReason(SignatureModifications.E),
                        TestPassed = testId % 2 == 0,
                        Deferred = false,
                        TestCaseId = testId
                    };
                    tests.Add(tc);
                }
            }

            return tvs;
        }
    }
}
