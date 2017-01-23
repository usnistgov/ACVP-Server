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
                    MessageLength = 160 + groupIdx * 2,
                    DigestLength = 160,
                    Tests = tests
                });
            }

            return testGroups;
        }
    }
}
