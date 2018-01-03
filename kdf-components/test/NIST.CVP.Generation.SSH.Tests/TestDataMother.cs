using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SSH.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SSH.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1)
        {
            if (groups > 4)
            {
                throw new Exception("Too many test groups");
            }

            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        K = new BitString("ABCD"),
                        H = new BitString("ABCD02"),
                        SessionId = new BitString("ABCD03"),
                       
                        InitialIvClient = new BitString("1AAADFF1"),
                        EncryptionKeyClient = new BitString("1AAADFF0"),
                        IntegrityKeyClient = new BitString("1AAADFFA"),
                        InitialIvServer = new BitString("1AAADFFB"),
                        EncryptionKeyServer = new BitString("1AAADFFC"),
                        IntegrityKeyServer = new BitString("1AAADFFC02"),

                        TestCaseId = testId,
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        HashAlg = new HashFunction(ModeValues.SHA2, DigestSizes.d256),
                        Cipher = (Cipher)groupIdx,
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }

            return testGroups;
        }
    }
}
