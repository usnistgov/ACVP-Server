using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CSHAKE.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm,
                    DigestSize = digestSize,
                    MessageLength = parameters.MessageLength.GetDeepCopy(),
                    HexCustomization = parameters.HexCustomization,
                    OutputLength = parameters.OutputLength.GetDeepCopy(),
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
