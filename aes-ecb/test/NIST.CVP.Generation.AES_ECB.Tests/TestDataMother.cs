using System.Collections.Generic;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB.Tests
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
                        PlainText = new BitString("1AAADFFF"),
                        Deferred = false,
                        FailureTest = failureTest,
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
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
                        PlainText = new BitString("1AAADFFF"),
                        CipherText = new BitString("7EADDC"),
                        Key = new BitString("9998ADCD"),
                        ResultsArray = new List<AlgoArrayResponse>()
                        {
                            new AlgoArrayResponse()
                            {
                                PlainText = new BitString("1AAADFFF"),
                                CipherText = new BitString("7EADDC"),
                                Key = new BitString("9998ADCD"),
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
