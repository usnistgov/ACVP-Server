﻿using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KMAC.v1_0
{
    public class TestGroupGeneratorMacVerification : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "mvt";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                if (parameters.NonXOF)
                {
                    var testGroup = new TestGroup
                    {
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        KeyLengths = parameters.KeyLen.GetDeepCopy(),
                        MacLengths = parameters.MacLen.GetDeepCopy(),
                        MessageLength = digestSize,
                        HexCustomization = parameters.HexCustomization,
                        XOF = false
                    };

                    testGroups.Add(testGroup);
                }

                if (parameters.XOF)
                {
                    var testGroupXOF = new TestGroup
                    {
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        KeyLengths = parameters.KeyLen.GetDeepCopy(),
                        MacLengths = parameters.MacLen.GetDeepCopy(),
                        MessageLength = digestSize,
                        HexCustomization = parameters.HexCustomization,
                        XOF = true
                    };

                    testGroups.Add(testGroupXOF);
                }
            }

            return testGroups;
        }
    }
}