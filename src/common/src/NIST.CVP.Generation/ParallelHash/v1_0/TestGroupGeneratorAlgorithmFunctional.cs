using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.ParallelHash.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGenerator<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public IEnumerable<TestGroup> BuildTestGroups(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                foreach (var xof in parameters.XOF)
                {
                    var testGroup = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        MessageLength = parameters.MessageLength.GetDeepCopy(),
                        HexCustomization = parameters.HexCustomization,
                        XOF = xof
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
