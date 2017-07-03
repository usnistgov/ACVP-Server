using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFB
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        private readonly List<(string testType, int numberOfKeys)> TestTypesAndNumberOfKeys = new List<(string testType, int numberOfKeys)>
        {
            ("Permutation", 1),
            //("InversePermutation", 1),
            ("SubstitutionTable", 1),
            ("VariableKey", 1),
            ("VariableText", 1),
            ("MultiBlockMessage", 2),
            ("MultiBlockMessage", 3),
            ("MCT", 2),
            ("MCT", 3)
        };

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var direction in parameters.Direction)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    // Encrypt Keying Option 2 is not valid, skip test groups
                    if (direction.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        continue;
                    }

                    var translatedKeyingOptionToNumberOfKeys = TdesHelpers.GetNumberOfKeysFromKeyingOption(keyingOption);

                    // Create groups for the 1 key (KATs) as well as the number of keys for the keying option
                    var testTypesToRun = TestTypesAndNumberOfKeys
                        .Where(w => w.numberOfKeys == translatedKeyingOptionToNumberOfKeys || w.numberOfKeys == 1);

                    AddTestGroups(testTypesToRun, direction, testGroups);
                }
            }
            return testGroups;
        }

        private void AddTestGroups(IEnumerable<(string testType, int numberOfKeys)> testTypesToRun, string direction, List<ITestGroup> testGroups)
        {
            foreach (var tup in testTypesToRun)
            {
                var testGroup = new TestGroup { Function = direction, NumberOfKeys = tup.numberOfKeys, TestType = tup.testType };
                testGroups.Add(testGroup);
            }
        }
    }
}