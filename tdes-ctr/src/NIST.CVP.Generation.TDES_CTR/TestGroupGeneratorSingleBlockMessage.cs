using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroupGeneratorSingleBlockMessage : ITestGroupGenerator<Parameters>
    {
        public const string LABEL = "singleblock";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
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

                        StaticGroupOfTests = false,
                        TestType = LABEL
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
