﻿using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "MCT";
        
        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
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

                    var translatedKeyingOptionToNumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption);

                    var tg = new TestGroup
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