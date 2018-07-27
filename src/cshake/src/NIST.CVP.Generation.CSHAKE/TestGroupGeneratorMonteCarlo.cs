using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
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
                    OutputLength = parameters.OutputLength,
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
