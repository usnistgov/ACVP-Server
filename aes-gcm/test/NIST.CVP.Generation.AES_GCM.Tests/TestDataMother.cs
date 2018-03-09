using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM.Tests
{
    public static class TestDataMother
    {
        public static TestVectorSet GetTestGroups(int groups, string direction, bool deferred, bool testPassed)
        {
            var testVectorSet = new TestVectorSet()
            {
                Algorithm = "AES",
                Mode = "GCM",
                IsSample = false
            };

            var testGroups = new List<TestGroup>();
            testVectorSet.TestGroups = testGroups;
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tg = new TestGroup
                {
                    AADLength = 16 + groupIdx * 2,
                    Function = direction,
                    IVLength = 96 + groupIdx * 2,
                    KeyLength = 256 + groupIdx * 2,
                    PTLength = 256 + groupIdx * 2,
                    TagLength = 16 + groupIdx * 2,
                    TestType = "AFT"
                };
                testGroups.Add(tg);

                var tests = new List<TestCase>();
                tg.Tests = tests;
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    var tc = new TestCase
                    {
                        IV = new BitString("00FF00FF"),
                        AAD = new BitString("0AAD"),
                        PlainText = new BitString("1AAADFFF"),
                        Deferred = deferred,
                        TestPassed = testPassed,
                        CipherText = new BitString("7EADDC"),
                        Tag = new BitString("1237AB"),
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId,
                        ParentGroup = tg
                    };
                    tests.Add(tc);
                }
            }
            return testVectorSet;
        }
    }
}
