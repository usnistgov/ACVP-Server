using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroupGeneratorCounter : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string LABEL = "counter";

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

                    if (parameters.OverflowCounter)
                    {
                        var overflowGroup = new TestGroup
                        {
                            Direction = direction,
                            NumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption),
                            IncrementalCounter = parameters.IncrementalCounter,
                            OverflowCounter = true,
                            TestType = LABEL
                        };

                        testGroups.Add(overflowGroup);
                    }

                    var testGroup = new TestGroup
                    {
                        Direction = direction,
                        NumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption),
                        IncrementalCounter = parameters.IncrementalCounter,
                        OverflowCounter = false,
                        TestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
