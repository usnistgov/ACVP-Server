﻿using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.SHA3.v1_0
{
    public class TestGroupGeneratorVariableOutput : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "VOT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            // VOT tests are only valid for shake
            if (!parameters.Algorithm.ToLower().Contains("shake"))
            {
                return testGroups;
            }

            foreach (var digestSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = "SHAKE",
                    DigestSize = digestSize,
                    IncludeNull = parameters.IncludeNull,
                    BitOrientedInput = parameters.BitOrientedInput,
                    BitOrientedOutput = parameters.BitOrientedOutput,
                    OutputLength = parameters.OutputLength.GetDeepCopy(),
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return testGroups;
        }
    }
}