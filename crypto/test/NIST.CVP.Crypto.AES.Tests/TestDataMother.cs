using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Tests.Fakes;

namespace NIST.CVP.Crypto.AES.Tests
{
    internal class TestDataMother
    {
        public List<FakeTestGroup> GetTestGroups(int groups = 1, string direction = "encrypt", bool failureTest = false)
        {
            var testGroups = new List<FakeTestGroup>();
            for (int groupIdx = 0; groupIdx < groups; groupIdx++)
            {

                var tests = new List<ITestCase>();
                for (int testId = 15 * groupIdx + 1; testId <= (groupIdx + 1) * 15; testId++)
                {
                    tests.Add(new FakeTestCase
                    {
                        Deferred = false,
                        FailureTest = failureTest,
                        TestCaseId = testId
                    });
                }

                testGroups.Add(
                    new FakeTestGroup()
                    {
                    }
                );
            }
            return testGroups;
        }
    }
}