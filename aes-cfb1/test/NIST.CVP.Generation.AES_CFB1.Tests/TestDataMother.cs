using System.Collections.Generic;
using NIST.CVP.Generation.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_CFB1.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        PlainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("1"),
                        Deferred = false,
                        FailureTest = failureTest,
                        CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("0"),
                        Key = new BitString("9998ADCD"),
                        IV = new BitString("CAFECAFE"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        
                        Function = direction,
                        KeyLength = 256 + groupIdx * 2,
                        Tests = tests,
                        TestType = "Sample"
                    }
                );
            }
            return testGroups;
        }

        public List<TestGroup> GetMCTTestGroups(int groups = 1, string direction = "encrypt")
        {
            List<TestGroup> testGroups = new List<TestGroup>();

            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Deferred = false,
                        PlainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("1"),
                        CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("0"),
                        Key = new BitString("9998ADCD"),
                        IV = new BitString("CAFECAFE"),
                        ResultsArray = new List<BitOrientedAlgoArrayResponse>()
                        {
                            new BitOrientedAlgoArrayResponse()
                            {
                                PlainText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("1"),
                                CipherText = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit("0"),
                                Key = new BitString("9998ADCD"),
                                IV = new BitString("CAFECAFE"),
                            }
                        },
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {

                        Function = direction,
                        KeyLength = 256 + groupIdx * 2,
                        Tests = tests,
                        TestType = "MCT"
                    }
                );
            }

            return testGroups;
        }
    }
}
