using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_ECB
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private readonly string[] _katTests = KatData.GetLabels();

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
                        TestType = katTest
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }
    }
}
