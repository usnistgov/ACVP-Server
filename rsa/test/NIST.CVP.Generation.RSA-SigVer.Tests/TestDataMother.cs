using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.RSA_SigVer.TestCaseExpectations;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("ABCD"),
                        Salt = new BitString("ABCD"),
                        Signature = new BitString("ABCD"),
                        Reason = new TestCaseExpectationReason(SignatureModifications.None),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d224),
                    Key = new KeyPair { PrivKey = new PrivateKey {D = 1, P = 2, Q = 3}, PubKey = new PublicKey {E = 4, N = 5}},
                    Mode = SignatureSchemes.Ansx931,
                    Modulo = 2048 + groupIdx,
                    TestType = "gdt",
                    SaltLen = 16,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
