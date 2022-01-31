using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.ConditioningComponents.Sp800_90B.Hash_DF
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        private const string TEST_TYPE = "AFT";

        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var hashAlg in capability.HashAlg)
                {
                    groups.Add(new TestGroup
                    {
                        HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg),
                        PayloadLength = capability.PayloadLen.GetDeepCopy(),
                        TestType = TEST_TYPE
                    });
                }
            }

            return Task.FromResult(groups);
        }
    }
}
