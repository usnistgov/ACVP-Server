using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_OFBI.v1_0
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        private readonly string[] _katTests = {
            "Permutation",
            "InversePermutation",
            "SubstitutionTable",
            "VariableKey",
            "VariableText"
        };

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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
