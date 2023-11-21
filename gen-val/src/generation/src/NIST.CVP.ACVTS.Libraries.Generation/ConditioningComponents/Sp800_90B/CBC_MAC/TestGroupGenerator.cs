using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.CBC_MAC
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();
            if (parameters.Keys != null)
            {
                foreach (var key in parameters.Keys)
                {
                    groups.Add(new TestGroup
                    {
                        PayloadLength = parameters.PayloadLen.GetDeepCopy(),
                        TestType = TEST_TYPE,
                        KeyLength = 0,
                        Key = key
                    });
                }
            }
            else
            {
                foreach (var keyLen in parameters.KeyLen)
                {
                    groups.Add(new TestGroup
                    {
                        PayloadLength = parameters.PayloadLen.GetDeepCopy(),
                        TestType = TEST_TYPE,
                        KeyLength = keyLen,
                        Key = null
                    });
                }
            }

            return Task.FromResult(groups);
        }
    }
}
