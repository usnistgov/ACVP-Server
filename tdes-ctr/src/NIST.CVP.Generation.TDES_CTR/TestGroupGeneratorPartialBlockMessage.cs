using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroupGeneratorPartialBlockMessage : ITestGroupGenerator<Parameters>
    {
        public const string LABEL = "partialblock";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            if (parameters.DataLength.ContainsValueOtherThan(128))
            {
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

                            // Only test case generator that cares about this information
                            DataLength = parameters.DataLength,

                            StaticGroupOfTests = false,
                            TestType = LABEL
                        };

                        testGroups.Add(testGroup);
                    }
                }
            }

            return testGroups;
        }
    }
}
