using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.CMAC_AES.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string direction = "gen", bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        FailureTest = failureTest,
                        Message = new BitString("FACE"),
                        Mac = new BitString("CAFE"),
                        Key = new BitString("9998ADCD"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        Function = direction,
                        KeyLength = 128 + groupIdx * 2,
                        MessageLength = groupIdx * 8,
                        MacLength = 64,
                        Tests = tests
                    }
                );
            }
            return testGroups;
        }
    }
}
