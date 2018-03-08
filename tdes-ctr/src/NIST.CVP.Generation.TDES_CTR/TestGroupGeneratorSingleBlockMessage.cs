using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroupGeneratorSingleBlockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "singleblock";

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
                        NumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption),
                        TestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
