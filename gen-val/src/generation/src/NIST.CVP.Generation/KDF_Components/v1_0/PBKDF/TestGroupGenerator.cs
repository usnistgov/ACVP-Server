using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KDF_Components.v1_0.PBKDF
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<IEnumerable<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();
            
            foreach (var capability in parameters.Capabilities)
            {
                capability.IterationCount.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.KeyLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.PasswordLength.SetRangeOptions(RangeDomainSegmentOptions.Random);
                capability.SaltLength.SetRangeOptions(RangeDomainSegmentOptions.Random);

                groups.AddRange(capability.HashAlg.Select(hashAlg => new TestGroup
                {
                    IterationCount = capability.IterationCount,
                    KeyLength = capability.KeyLength,
                    PasswordLength = capability.PasswordLength,
                    SaltLength = capability.SaltLength,
                    HashAlg = ShaAttributes.GetHashFunctionFromName(hashAlg)
                }));
            }

            return Task.FromResult(groups.AsEnumerable());
        }
    }
}