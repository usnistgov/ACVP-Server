using NIST.CVP.Generation.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.ParallelHash.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                foreach (var xof in parameters.XOF)
                {
                    var testGroup = new TestGroup
                    {
                        Function = "ParallelHash",
                        DigestSize = digSize,
                        OutputLength = parameters.OutputLength.GetDeepCopy(),
                        TestType = TEST_TYPE,
                        HexCustomization = parameters.HexCustomization,
                        BlockSize = parameters.BlockSize.GetDeepCopy(),
                        XOF = xof
                    };

                    testGroups.Add(testGroup);
                }
            }

            return Task.FromResult(testGroups);
        }
    }
}
