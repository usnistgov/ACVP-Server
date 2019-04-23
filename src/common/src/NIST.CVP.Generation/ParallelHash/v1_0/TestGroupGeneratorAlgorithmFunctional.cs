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
                if (parameters.NonXOF)
                {
                    var testGroup = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        MessageLength = parameters.MessageLength.GetDeepCopy(),
                        HexCustomization = parameters.HexCustomization,
                        XOF = false
                    };

                    testGroups.Add(testGroup);
                }

                if (parameters.XOF)
                {
                    var testGroupXOF = new TestGroup
                    {
                        Function = parameters.Algorithm,
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        MessageLength = parameters.MessageLength.GetDeepCopy(),
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
