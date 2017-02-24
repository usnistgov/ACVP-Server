using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestVectorFactory : ITestVectorFactory<Parameters>
    {
        public readonly List<Tuple<string, int>> TestTypesAndNumberOfKeys = new List<Tuple<string, int>>
        {
            new Tuple<string, int>("Permutation", 1),
            new Tuple<string, int>("InversePermutation", 1),
            new Tuple<string, int>("SubstitutionTable", 1),
            new Tuple<string, int>("VariableKey", 1),
            new Tuple<string, int>("VariableText", 1),
            new Tuple<string, int>("MultiBlockMessage", 2),
            new Tuple<string, int>("MultiBlockMessage", 3),
            //new Tuple<string, int>("MonteCarlo", 2),
            //new Tuple<string, int>("MonteCarlo", 3) Be sure to add back with MCT

        };
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            var testVector = new TestVectorSet { TestGroups = groups, Algorithm = "TDES-CBC", IsSample = parameters.IsSample };

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {

            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                foreach (var keyingOption in parameters.KeyingOption)
                {
                    // Encrypt Keying Option 2 is not valid, skip test groups
                    if (function.ToLower() == "encrypt" && keyingOption == 2)
                    {
                        continue;
                    }

                    var translatedKeyingOptionToNumberOfKeys = GetNumberOfKeysFromKeyingOption(keyingOption);

                    // Create groups for the 1 key (KATs) as well as the number of keys for the keying option
                    var testTypesToRun = TestTypesAndNumberOfKeys
                        .Where(w => w.Item2 == translatedKeyingOptionToNumberOfKeys || w.Item2 == 1);

                    AddTestGroups(testTypesToRun, function, testGroups);
                }
            }
            return testGroups;
        }

        private int GetNumberOfKeysFromKeyingOption(int keyingOption)
        {
            switch (keyingOption)
            {
                case 1:
                    return 3;
                case 2:
                    return 2;
                default:
                    throw new ArgumentException(nameof(keyingOption));
            }
        }

        private void AddTestGroups(IEnumerable<Tuple<string, int>> testTypesToRun, string function, List<ITestGroup> testGroups)
        {
            foreach (var tup in testTypesToRun)
            {
                var testGroup = new TestGroup { Function = function, NumberOfKeys = tup.Item2, TestType = tup.Item1 };
                testGroups.Add(testGroup);
            }
        }
    }
}
