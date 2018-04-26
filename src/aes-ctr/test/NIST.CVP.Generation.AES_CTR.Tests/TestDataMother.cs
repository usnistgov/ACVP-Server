using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string direction = "encrypt", string testType = "aft")
        {
            var tvs = new TestVectorSet
            {
                Algorithm = "TDES",
                IsSample = true,
                Mode = "CTR"
            };

            var testGroups = new List<TestGroup>();
            tvs.TestGroups = testGroups;
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Direction = direction,
                    KeyLength = 128,
                    TestType = testType,
                    IncrementalCounter = true,
                    OverflowCounter = false
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        ParentGroup = tg,
                        PlainText = new BitString("1AAADFFF"),
                        Deferred = true,
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD9998ADCD9998ADCD9998ADCD9998ADCD9998ADCD"),
                        IV = new BitString("12314143"),
                        Length = 10,
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("counter", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.IVs = new List<BitString>
                        {
                            new BitString("01"),
                            new BitString("02"),
                            new BitString("03")
                        };
                        tc.IV = null;
                    }
                }
            }

            return tvs;
        }
    }
}
