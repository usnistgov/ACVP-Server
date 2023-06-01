using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;

namespace NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v2_0
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var list = new List<TestGroup>();

            var keyBlockLengthMinMax = parameters.KeyBlockLength.GetDomainMinMaxAsEnumerable().Distinct().ToList();
            
            foreach (var tlsMode in parameters.TlsVersion)
            {
                foreach (var keyBlockLength in keyBlockLengthMinMax)
                {
                    if (tlsMode == TlsModes.v10v11)
                    {
                        list.Add(new TestGroup
                        {
                            HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                            TlsMode = tlsMode,
                            KeyBlockLength = keyBlockLength,
                            PreMasterSecretLength = 384
                        });
                    }
                    else
                    {
                        foreach (var hashAlg in parameters.HashAlg)
                        {
                            var hashFunction = ShaAttributes.GetHashFunctionFromName(hashAlg);
                            list.Add(new TestGroup
                            {
                                HashAlg = hashFunction,
                                TlsMode = tlsMode,
                                KeyBlockLength = keyBlockLength,
                                PreMasterSecretLength = 384
                            });
                        }
                    }
                }
            }

            return Task.FromResult(list);
        }
    }
}
