using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using NIST.CVP.Common;
using NIST.CVP.Common.Helpers;

namespace NIST.CVP.Generation.TDES_CFB
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _katTests = new string[]
        {
            "Permutation",
            "InversePermutation",
            "SubstitutionTable",
            "VariableKey",
            "VariableText"
        };

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode);
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var katTest in _katTests)
                {
                    var tg = new TestGroup
                    {
                        AlgoMode = algoMode,
                        Function = function,
                        KeyingOption = 1,
                        TestType = katTest
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }
    }
}