using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CBCI.Tests
{
    public class TestDataMother
    {
        private string direction;

        public List<TestGroup> GetTestGroups(int groups = 1, string direction1 = "encrypt", string direction2 = "decrypt", bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {

                        PlainText = new BitString("1AAADFFF1AAADFFF"),
                        Deferred = false,
                        FailureTest = failureTest,
                        CipherText = new BitString("1164a939c1936151"),
                        Key1 = new BitString("9998ADCD9998ADCD"),
                        Key2 = new BitString("9998ADCD9998ADCD"),
                        Key3 = new BitString("9998ADCD9998ADCD"),
                        TestCaseId = testId
                    });
                }
                if (groupIdx % 2 == 0)
                {
                    direction = direction1;
                }
                else
                {
                    direction = direction2;
                }
                testGroups.Add(
                    new TestGroup
                    {

                        Function = direction,
                        TestType = "MultiBlockMessage",
                        //NumberOfKeys = groupIdx+1,
                        Tests = tests
                    }
                );
            }
            return testGroups;
        }
    }
}
