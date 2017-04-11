using System.Collections.Generic;
using NIST.CVP.Crypto.SHA2;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(HashFunction hashFunction, int groups = 1, bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for(int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for(int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Message = new BitString("BEEFFACE"),
                        Deferred = false,
                        FailureTest = failureTest,
                        Digest = new BitString("FACEDAD1"),
                        TestCaseId = testId
                    });
                }

                testGroups.Add(new TestGroup
                {
                    Function = hashFunction.Mode,
                    DigestSize = hashFunction.DigestSize + groupIdx,        // This is bad and will fail for many groups but needs to be done to separate the tests based on different hashes...
                    TestType = "aft",
                    BitOriented = false,
                    IncludeNull = false,
                    Tests = tests
                });
            }

            return testGroups;
        }

        public List<TestGroup> GetMCTTestGroups(int groups = 1)
        {
            var testGroups = new List<TestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new TestCase
                    {
                        Deferred = false,
                        ResultsArray = new List<AlgoArrayResponse>
                        {
                            new AlgoArrayResponse
                            {
                                Message = new BitString("DEADBEEF"),
                                Digest = new BitString("ADD4FACE")
                            },
                            new AlgoArrayResponse
                            {
                                Message = new BitString("DEADBEEF02"),
                                Digest = new BitString("ADD4FACE02")
                            }
                        },
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new TestGroup
                    {
                        Function = ModeValues.SHA2,
                        DigestSize = DigestSizes.d224,
                        Tests = tests,
                        TestType = "mct"
                    }
                );
            }

            return testGroups;
        }
    }
}
