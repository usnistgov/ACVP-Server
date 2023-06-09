using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.BlockCipher_DF
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            var minMaxOutputLen = parameters.OutputLen.GetDomainMinMaxAsEnumerable();

            foreach (var outputLen in minMaxOutputLen)
            {
                foreach (var keyLen in parameters.KeyLen)
                {
                    groups.Add(new TestGroup
                    {
                        OutputLen = outputLen,
                        PayloadLen = parameters.PayloadLen.GetDeepCopy(),
                        KeyLength = keyLen,
                        TestType = TEST_TYPE
                    });
                }
            }
            
            return Task.FromResult(groups);
        }
    }
}
