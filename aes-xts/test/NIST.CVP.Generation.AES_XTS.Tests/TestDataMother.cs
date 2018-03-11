using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_XTS.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, string direction, string tweakMode)
        {
            var testVectorSet = new TestVectorSet()
            {
                Algorithm = "AES",
                Mode = "XTS",
                IsSample = false
            };

            var testGroups = new List<TestGroup>();
            testVectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Direction = direction,
                    KeyLen = 52,
                    PtLen = 128,
                    TweakMode = tweakMode,
                    TestType = "AFT"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        PlainText = new BitString("1AAADFFF"),
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId,
                        ParentGroup = tg,
                        // note not actually true in real scenarios, 
                        // just want to be able to confirm the property isn't serialized.
                        Deferred = true 
                    };

                    if (tweakMode == "hex")
                    {
                        tc.I = new BitString("CAFECAFE");
                    }

                    if (tweakMode == "number")
                    {
                        tc.SequenceNumber = 10;
                    }

                    tests.Add(tc);
                }
            }
            return testVectorSet;
        }
    }
}
