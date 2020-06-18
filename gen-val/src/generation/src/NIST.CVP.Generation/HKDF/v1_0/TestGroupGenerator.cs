using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.HKDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                capability.InfoLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.SaltLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.InputKeyingMaterialLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.KeyLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
            
                foreach (var hmacAlg in capability.HmacAlg)
                {
                    var hmac = ShaAttributes.GetHashFunctionFromName(hmacAlg);
                    var group = new TestGroup
                    {
                        HmacAlg = hmac
                    };
                
                    groups.Add(group);
                }
            }
            
            return Task.FromResult(groups);
        }
    }
}