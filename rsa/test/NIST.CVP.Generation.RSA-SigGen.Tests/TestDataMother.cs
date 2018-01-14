using NIST.CVP.Crypto.RSA;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Hash.SHA2;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for(var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("ABCD"),
                        Salt = new BitString("ABCD"),
                        Signature = new BitString("ABCD"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    HashAlg = new HashFunction { Mode = ModeValues.SHA2, DigestSize = DigestSizes.d224 },
                    Key = new KeyPair(31, 29, 7),
                    Mode = SigGenModes.ANS_931,
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
