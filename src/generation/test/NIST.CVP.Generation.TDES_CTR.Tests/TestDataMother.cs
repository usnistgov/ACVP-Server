using NIST.CVP.Generation.TDES_CTR.v1_0;
using NIST.CVP.Math;
using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CTR.Tests
{
    public class TestDataMother
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
                    KeyingOption = groupIdx + 1,
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
                        Iv = new BitString("12314143"),
                        PayloadLen = 10,
                        TestCaseId = testId
                    };
                    tests.Add(tc);

                    if (testType.Equals("counter", StringComparison.OrdinalIgnoreCase))
                    {
                        tc.Iv = null;
                    }
                }
            }

            return tvs;
        }
    }
}
