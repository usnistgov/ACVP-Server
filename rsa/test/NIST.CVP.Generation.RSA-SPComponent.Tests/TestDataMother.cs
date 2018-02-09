using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.RSA2.Enums;
using NIST.CVP.Crypto.RSA2.Keys;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_SPComponent.Tests
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
                        Signature = new BitString("ABCD"),
                        Key = new KeyPair { PubKey = new PublicKey {E = 1, N = 2}, PrivKey = new PrivateKey{D = 3, P = 4, Q = 5}},
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    Modulo = 2048 + groupIdx,
                    KeyFormat = PrivateKeyModes.Standard,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
