﻿using System.Collections.Generic;
using NIST.CVP.Crypto.Common.Symmetric.TDES.KATs;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CBCI.v1_0
{
    public class TestGroupGeneratorKnownAnswer : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "KAT";
        private readonly string[] _katTests = KatData.GetLabels();

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();
            foreach (var function in parameters.Direction)
            {
                foreach (var katTest in _katTests)
                {
                    var tg = new TestGroup()
                    {
                        Function = function,
                        KeyingOption = 3,
                        InternalTestType = katTest
                    };

                    testGroups.Add(tg);
                }
            }
            return testGroups;
        }
    }
}