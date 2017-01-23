using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_ECB
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
            new Tuple<string, int>("MonteCarlo", 2),
            new Tuple<string, int>("MonteCarlo", 3)

        };
        public ITestVectorSet BuildTestVectorSet(Parameters parameters)
        {
            var groups = BuildTestGroups(parameters);
            var testVector = new TestVectorSet {TestGroups = groups, Algorithm = "TDES-ECB", IsSample = parameters.IsSample};

            return testVector;
        }

        private List<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Mode)
            {
                var testTypesToRun = TestTypesAndNumberOfKeys;
                if (function.ToLower() == "encrypt")
                {
                    //only do the KnownAnswer (KAT) 1 key tests and 3 key tests for encryption
                   testTypesToRun = TestTypesAndNumberOfKeys.Where(t => t.Item2 != 2).ToList();
                    
                }
                AddTestGroups(testTypesToRun, function, testGroups);
            }
            return testGroups;
        }

        private void AddTestGroups(IEnumerable<Tuple<string, int>> testTypesToRun, string function, List<ITestGroup> testGroups)
        {
            foreach (var tup  in testTypesToRun)
            {
                var testGroup = new TestGroup {Function = function, NumberOfKeys = tup.Item2, TestType = tup.Item1};
                testGroups.Add(testGroup);
            }
        }
    }
}
