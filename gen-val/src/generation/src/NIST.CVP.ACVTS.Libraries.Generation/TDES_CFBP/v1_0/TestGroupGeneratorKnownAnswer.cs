﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Common.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.TDES_CFBP.v1_0
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";
        private readonly string[] _katTests = KatData.GetLabels();

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
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

            return Task.FromResult(testGroups);
        }
    }
}
