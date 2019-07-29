using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Generation.AES_CFB1.v1_0;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups = 1, string direction = "encrypt", string testType = "aft")
        {
            var vectorSet = new TestVectorSet();

            var testGroups = new List<TestGroup>();
            vectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tg = new TestGroup
                {
                    TestGroupId = 44,
                    Function = direction,
                    KeyLength = 256 + groupIdx * 2,
                    TestType = testType
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        PlainText = new BitString("AA"),
                        CipherText = new BitString("BB"),
                        Key = new BitString("9998ADCD"),
                        IV = new BitString("CAFECAFE"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    };
                    tests.Add(tc);

                    if (testType.ToLower() == "mct")
                    {
                        tc.ResultsArray = new List<AlgoArrayResponse>
                        {
                            new AlgoArrayResponse
                            {
                                CipherText = new BitString("01"),
                                IV = new BitString("02"),
                                Key = new BitString("03"),
                                PlainText = new BitString("04")
                            }
                        };
                    }
                }
            }
            return vectorSet;
        }
    }
}
