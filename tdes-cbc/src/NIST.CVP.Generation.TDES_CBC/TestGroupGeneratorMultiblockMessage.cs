using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestGroupGeneratorMultiblockMessage : ITestGroupGenerator<Parameters>
    {
        private const string TEST_TYPE = "MultiBlockMessage";

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    // Encrypt Keying Option 2 is not valid, skip test groups
                    if (function.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        continue;
                    }

                    var translatedKeyingOptionToNumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption);

                    TestGroup tg = new TestGroup()
                    {
                        Function = function,
                        NumberOfKeys = translatedKeyingOptionToNumberOfKeys,
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }
            return testGroups;
        }

    }
}