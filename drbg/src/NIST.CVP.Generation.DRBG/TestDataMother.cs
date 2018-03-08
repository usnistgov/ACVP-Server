using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DRBG
{
    public static class TestDataMother
    {
        public static TestVectorSet GetSampleVectorSet(
            int numGroups = 1, 
            int numTestCases = 1, 
            bool reseed = false,
            bool predictionResistance = false
        )
        {
            TestVectorSet testVectorSet = new TestVectorSet()
            {
                Algorithm = "drbg",
                Mode = "ctr"
            };

            List<ITestGroup> groups = new List<ITestGroup>();
            testVectorSet.TestGroups = groups;

            for (int groupI = 0; groupI < numGroups; groupI++)
            {
                var group = new TestGroup()
                {
                    AdditionalInputLen = 42,
                    DerFunc = true,
                    PersoStringLen = 50,
                    EntropyInputLen = 64,
                    NonceLen = 55,
                    PredResistance = predictionResistance,
                    ReSeed = reseed
                };

                groups.Add(group);

                List<ITestCase> tests = new List<ITestCase>();
                group.Tests = tests;
                for (int testI = 0; testI < numTestCases; testI++)
                {
                    var test = new TestCase()
                    {

                    };

                    tests.Add(test);
                }
            }

            return testVectorSet;
        }
    }
}