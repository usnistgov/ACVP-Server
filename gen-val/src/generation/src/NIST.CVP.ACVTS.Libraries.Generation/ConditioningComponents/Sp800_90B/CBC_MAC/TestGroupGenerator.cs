using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.CBC_MAC
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();
            foreach (var keyLen in parameters.KeyLen)
            {
                groups.Add(new TestGroup
                {
                    KeyLength = keyLen,
                    PayloadLength = parameters.PayloadLen.GetDeepCopy(),
                    TestType = TEST_TYPE
                });
            }

            return Task.FromResult(groups);
        }
    }
}
