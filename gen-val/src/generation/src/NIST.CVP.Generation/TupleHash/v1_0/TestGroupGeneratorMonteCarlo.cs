using NIST.CVP.Generation.Core;
using System.Collections.Generic;

namespace NIST.CVP.Generation.TupleHash.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                foreach (var xof in parameters.XOF)
                {
                    var testGroup = new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digSize,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        MessageLength = parameters.MessageLength.GetDeepCopy(),
                        TestType = TEST_TYPE,
                        XOF = xof
                    };

                    testGroups.Add(testGroup);
                }
            }

            return testGroups;
        }
    }
}
