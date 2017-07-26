using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;
using System.Text;

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
                    Modulo = 1 + groupIdx,
                    TestType = "gdt",
                    Salt = new BitString("ABCD"),
                    SaltLen = 16,
                    SaltMode = SaltModes.FIXED,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
