﻿using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestGroupGeneratorMultiblockMessage : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "MultiBlockMessage";

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

                    var tg = new TestGroup()
                    {
                        AlgoMode = algoMode,
                        Function = function,
                        KeyingOption = keyingOption,
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }
    }
}