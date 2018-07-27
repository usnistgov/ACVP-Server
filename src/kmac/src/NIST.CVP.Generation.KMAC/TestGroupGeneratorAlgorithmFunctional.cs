using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KMAC
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "aft";

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
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        TestType = TEST_TYPE,
                        KeyLengths = parameters.KeyLen.GetDeepCopy(),
                        MacLengths = parameters.MacLen.GetDeepCopy(),
                        HexCustomization = parameters.HexCustomization,
                        IncludeNull = parameters.IncludeNull,
                        XOF = false
                    };

                    testGroups.Add(testGroup);
                }

                if (parameters.XOF)
                {
                    var testGroupXOF = new TestGroup
                    {
                        DigestSize = digestSize,
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        TestType = TEST_TYPE,
                        KeyLengths = parameters.KeyLen.GetDeepCopy(),
                        MacLengths = parameters.MacLen.GetDeepCopy(),
                        HexCustomization = parameters.HexCustomization,
                        IncludeNull = parameters.IncludeNull,
                        XOF = true
                    };

                    testGroups.Add(testGroupXOF);
                }
            }

            return testGroups;
        }
    }
}
