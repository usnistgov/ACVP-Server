using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.CSHAKE
{
    public class TestGroupGeneratorVariableOutput : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "vot";
        private const string TEST_TYPE_SHAKE = "votshake";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroupShake = new TestGroup
                {
                    Function = parameters.Algorithm,
                    DigestSize = digestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    OutputLength = parameters.OutputLength.GetDeepCopy(),
                    TestType = TEST_TYPE_SHAKE
                };

                testGroups.Add(testGroupShake);

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
