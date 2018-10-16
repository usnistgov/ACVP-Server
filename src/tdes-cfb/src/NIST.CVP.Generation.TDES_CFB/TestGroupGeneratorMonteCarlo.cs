using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "MCT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode);
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    // Encrypt Keying Option 2 is not valid, skip test groups
                    if (function.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        continue;
                    }

                    var tg = new TestGroup
                    {
                        AlgoMode = algoMode,
                        Function = function,
                        KeyingOption = keyingOption,
                        TestType = TEST_TYPE,
                        InternalTestType = TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }
    }
}