using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.ParallelHash
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                if (parameters.NonXOF)
                {
                    var testGroup = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digSize,
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        OutputLength = parameters.OutputLength,
                        TestType = TEST_TYPE,
                        XOF = false
                    };

                    testGroups.Add(testGroup);
                }

                if (parameters.XOF)
                {
                    var testGroupXOF = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digSize,
                        BitOrientedInput = parameters.BitOrientedInput,
                        BitOrientedOutput = parameters.BitOrientedOutput,
                        OutputLength = parameters.OutputLength,
                        TestType = TEST_TYPE,
                        XOF = true
                    };

                    testGroups.Add(testGroupXOF);
                }
            }

            return testGroups;
        }
    }
}
