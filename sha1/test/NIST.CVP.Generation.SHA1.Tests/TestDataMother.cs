using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA1.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(int groups = 1, bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("0AAD"),
                        Deferred = false,
                        FailureTest = failureTest,
                        Digest = new BitString("ABCD"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    BitOriented = false,
                    TestType = "Sample",
                    IncludeNull = false,
                    MessageLength = 160 + groupIdx * 2,
                    DigestLength = 160,
                    Tests = tests
                });
            }

            return testGroups;
        }

        public List<TestGroup> GetMCTTestGroups(int groups = 1)
        {
            List<TestGroup> testGroups = new List<TestGroup>();

            for(int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for(int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Deferred = false,
                        ResultsArray = new List<AlgoArrayResponse>()
                        {
                            new AlgoArrayResponse()
                            {
                                Message = new BitString("AABBCCDDEEFF"),
                                Digest = new BitString("112233445566")
                            }
                        },
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    BitOriented = false,
                    DigestLength = 160,
                    IncludeNull = false,
                    MessageLength = 10 + groupIdx * 2,
                    Tests = tests,
                    TestType = "MCT"
                });
            }

            return testGroups;
        }
    }
}
