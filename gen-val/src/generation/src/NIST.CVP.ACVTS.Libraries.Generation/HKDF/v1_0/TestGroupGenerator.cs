using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Math.Domain;

namespace NIST.CVP.ACVTS.Libraries.Generation.HKDF.v1_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new List<TestGroup>();

            foreach (var capability in parameters.Capabilities)
            {
                foreach (var hmacAlg in capability.HmacAlg)
                {
                    var hmac = ShaAttributes.GetHashFunctionFromName(hmacAlg);
                    var group = new TestGroup
                    {
                        HmacAlg = hmac,
                        InputKeyingMaterialLength = capability.InputKeyingMaterialLength.GetDeepCopy(),
                        OtherInfoLength = capability.InfoLength.GetDeepCopy(),
                        SaltLength = capability.SaltLength.GetDeepCopy(),
                        KeyLength = capability.KeyLength.GetDeepCopy()
                    };

                    groups.Add(group);
                }
            }

            return Task.FromResult(groups);
        }
    }
}
