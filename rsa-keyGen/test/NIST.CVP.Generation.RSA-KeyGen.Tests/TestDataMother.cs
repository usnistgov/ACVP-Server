using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
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
                        Key = new KeyPair(31, 39, 7),
                        Seed = new BitString("BEEFFACE"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    HashAlg = new HashFunction { DigestSize = DigestSizes.d224, Mode = ModeValues.SHA2},
                    InfoGeneratedByServer = true,
                    Mode = KeyGenModes.B32,
                    Modulo = 1 + groupIdx,
                    TestType = "aft",
                    PubExp = PubExpModes.RANDOM,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
