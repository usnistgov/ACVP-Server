using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters>
    {
        public const string TEST_TYPE = "MCT";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm,
                    DigestSize = digSize,
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
