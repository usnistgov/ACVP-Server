using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.SHA3
{
    public class TestGroupGeneratorVariableOutput : ITestGroupGenerator<Parameters>
    {
        private const string TEST_TYPE = "vot";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            // VOT tests are only valid for shake
            if (parameters.Algorithm.ToLower() != "shake")
            {
                return testGroups;
            }

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = parameters.Algorithm,
                    DigestSize = digestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    OutputLength = parameters.OutputLength.GetDeepCopy(),
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}
