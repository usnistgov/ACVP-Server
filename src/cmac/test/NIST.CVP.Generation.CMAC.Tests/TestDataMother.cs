using System.Collections.Generic;
using NIST.CVP.Generation.CMAC.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, string direction, bool testPassed)
        {
            var testVectorSet = new TestVectorSet()
            {
                Algorithm = "CMAC",
                Mode = "AES",
                IsSample = false
            };

            var testGroups = new List<TestGroup>();
            testVectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    Function = direction,
                    KeyLength = 256 + groupIdx * 2,
                    MessageLength = 42,
                    MacLength = 55,
                    TestType = "AFT"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        TestPassed = testPassed,
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId,
                        Message = new BitString("BEEFFACE"),
                        Mac = new BitString("FACEBEEF"),
                        ParentGroup = tg
                    };
                    
                    tests.Add(tc);
                }
            }
            return testVectorSet;
        }
    }
}
