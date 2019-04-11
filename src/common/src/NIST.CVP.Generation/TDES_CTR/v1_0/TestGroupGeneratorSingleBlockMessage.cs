using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR.v1_0
{
    public class TestGroupGeneratorSingleBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        private const string INTERNAL_TEST_TYPE = "singleblock";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var direction in parameters.Direction)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    if (direction.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        // Don't allow encrypt on key option 2
                        continue;
                    }

                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        KeyingOption = keyingOption,
                        TestType = TEST_TYPE,
                        InternalTestType = INTERNAL_TEST_TYPE
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
