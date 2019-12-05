using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR.v1_0
{
    public class TestGroupGeneratorKnownAnswerTest : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        private readonly string[] _katTests =
        {
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
                        Direction = function,
                        KeyingOption = 1,
                        TestType = TEST_TYPE,
                        InternalTestType = katTest
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }
    }
}
