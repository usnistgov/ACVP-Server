﻿using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class TestGroupGeneratorKnownAnswerTest : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
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
                        NumberOfKeys = 1,
                        TestType = katTest,
                    };

                    testGroups.Add(tg);
                }
            }

            return testGroups;
        }
    }
}