using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGenerator<Parameters>
    {
        private const string TEST_TYPE = "aft";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm,
                    DigestSize = digestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    MinOutputLength = parameters.MinOutputLength,
                    MaxOutputLength = parameters.MaxOutputLength,
                    TestType = TEST_TYPE
                };
                    
                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
