using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm.ToLower().Contains("shake") ? "SHAKE" : "SHA3",
                    DigestSize = digestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
