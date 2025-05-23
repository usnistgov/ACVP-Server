﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0
{
    public class TestGroupGeneratorMultiblockMessage : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";
        private const string INTERNAL_TEST_TYPE = "MultiBlockMessage";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
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
                        TestType = TEST_TYPE,
                        InternalTestType = INTERNAL_TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
