using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TDES_CFBP
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";
        private readonly string[] _katTests = KatData.GetLabels();

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var algoMode = AlgoModeHelpers.GetAlgoModeFromAlgoAndMode(parameters.Algorithm, parameters.Mode, parameters.Revision);
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var katTest in _katTests)
                {
                    var tg = new TestGroup()
                    {
                        AlgoMode = algoMode,
                        Function = function,
                        KeyingOption = 1,
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