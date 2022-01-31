using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.AnsiX942.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var groups = new HashSet<TestGroup>();
            foreach (var mode in parameters.KdfType)
            {
                // OID presence is only enforced on DER
                if (mode == AnsiX942Types.Der)
                {
                    foreach (var oid in parameters.Oid)
                    {
                        foreach (var hashAlg in parameters.HashAlg)
                        {
                            var hash = ShaAttributes.GetHashFunctionFromName(hashAlg);
                            var group = new TestGroup
                            {
                                HashAlg = hash,
                                KdfType = mode,
                                Oid = AnsiX942OidHelper.GetOidFromEnum(oid),
                                KeyLen = parameters.KeyLen,
                                OtherInfoLen = parameters.OtherInfoLen,
                                SuppInfoLen = parameters.SuppInfoLen,
                                ZzLen = parameters.ZzLen
                            };

                            groups.Add(group);
                        }
                    }
                }
                else
                {
                    foreach (var hashAlg in parameters.HashAlg)
                    {
                        var hash = ShaAttributes.GetHashFunctionFromName(hashAlg);
                        var group = new TestGroup
                        {
                            HashAlg = hash,
                            KdfType = mode,
                            KeyLen = parameters.KeyLen,
                            OtherInfoLen = parameters.OtherInfoLen,
                            SuppInfoLen = parameters.SuppInfoLen,
                            ZzLen = parameters.ZzLen
                        };

                        groups.Add(group);
                    }
                }
            }

            return Task.FromResult(groups.ToList());
        }
    }
}
