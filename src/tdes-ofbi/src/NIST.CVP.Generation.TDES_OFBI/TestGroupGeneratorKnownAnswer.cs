using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_OFBI
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        private readonly string[] _katTests = {
            "Permutation",
            "InversePermutation",
            "SubstitutionTable",
            "VariableKey",
            "VariableText"
        };

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var katTest in _katTests)
                {
                    var tg = new TestGroup
                    {
                        Function = function,
                        KeyingOption = 3,
                        InternalTestType = katTest,
                        TestType = TEST_TYPE
                    };

                    testGroups.Add(tg);
                }
            }
            return testGroups;
        }
    }
}
