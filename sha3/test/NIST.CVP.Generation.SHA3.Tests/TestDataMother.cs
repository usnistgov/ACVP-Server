using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA3.Tests
{
    public class TestDataMother
    {
        public List<TestGroup> GetTestGroups(HashFunction hashFunction, int groups = 1, bool failureTest = false)
        {
            var testGroups = new List<TestGroup>();
            for (var groupIdx = 0; groupIdx < groups; groupIdx++)
            {
                var tests = new List<ITestCase>();
                for (var testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
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
                    Function = hashFunction.XOF ? "SHAKE" : "SHA3",
                    DigestSize = hashFunction.DigestSize,
                    TestType = "AFT",
                    BitOrientedInput = false,
                    BitOrientedOutput = false,
                    IncludeNull = false,
                    MaxOutputLength = 4000 + groupIdx,
                    MinOutputLength = 16 + groupIdx,
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
                        Function = "SHA3",
                        DigestSize = 224,
                        Tests = tests,
                        TestType = "mct"
                    }
                );
            }

            return testGroups;
        }
    }
}
