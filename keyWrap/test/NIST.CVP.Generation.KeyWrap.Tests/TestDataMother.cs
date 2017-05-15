using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KeyWrap.Tests
{
    internal class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 2, string direction = "encrypt", bool isFailureTest = false)
        {
            List<TestGroup> list = new List<TestGroup>();

            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                TestGroup tg = new TestGroup()
                {
                    Direction = direction,
                    PtLen = 128 * (groupIdx + 1),
                    KeyLength = 128,
                    KwCipher = "cipher",
                    TestType = "AFT",
                };

                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    TestCase tc = new TestCase()
                    {
                        Key = new BitString(128),
                        PlainText = new BitString(128),
                        CipherText = new BitString(128),
                        FailureTest = isFailureTest,
                        TestCaseId = testId
                    };

                    tg.Tests.Add(tc);
                }

                list.Add(tg);
            }

            return list;
        }
    }
}