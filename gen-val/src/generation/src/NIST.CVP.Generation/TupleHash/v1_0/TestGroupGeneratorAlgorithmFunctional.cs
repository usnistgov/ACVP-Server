using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.TupleHash.v1_0
{
    public class TestGroupGeneratorAlgorithmFunctional : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digestSize in parameters.DigestSizes)
            {
                foreach (var xof in parameters.XOF)
                {
                    var testGroup = new TestGroup
                    {
                        Function = "TupleHash",
                        DigestSize = digestSize,
                        TestType = TEST_TYPE,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        MessageLength = parameters.MessageLength.GetDeepCopy(),
                        XOF = xof,
                        HexCustomization = parameters.HexCustomization
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups.AsEnumerable());
        }
    }
}
