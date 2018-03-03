using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBC
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGenerator<Parameters>
    {
        private readonly string[] _katTests = new string[]
        {
            "Permutation",
            "InversePermutation",
            "SubstitutionTable",
            "VariableKey",
            "VariableText"
        };

        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<ITestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var katTest in _katTests)
                { 
                    TestGroup tg = new TestGroup()
                    {
                        Function = function,
                        KeyingOption = 3,
                        TestType = katTest
                    };

                    testGroups.Add(tg);
                }
            }
            return testGroups;
        }
    }
}
