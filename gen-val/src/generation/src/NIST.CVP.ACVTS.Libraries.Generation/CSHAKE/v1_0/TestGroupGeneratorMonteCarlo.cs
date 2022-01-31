using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.CSHAKE.v1_0
{
    public class TestGroupGeneratorMonteCarlo : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public const string TEST_TYPE = "MCT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var testGroups = new List<TestGroup>();

            foreach (var digSize in parameters.DigestSizes)
            {
                var testGroup = new TestGroup
                {
                    Function = "cSHAKE",
                    DigestSize = digSize,
                    MessageLength = parameters.MessageLength.GetDeepCopy(),
                    OutputLength = parameters.OutputLength.GetDeepCopy(),
                    TestType = TEST_TYPE
                };

                testGroups.Add(testGroup);
            }

            return Task.FromResult(testGroups);
        }
    }
}
