using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KDF_Components.v1_0.TLS
{
    public class TestGroupGenerator : ITestGroupGeneratorAsync<Parameters, TestGroup, TestCase>
    {
        public Task<List<TestGroup>> BuildTestGroupsAsync(Parameters parameters)
        {
            var list = new List<TestGroup>();

            foreach (var tlsMode in parameters.TlsVersion)
            {
                var modeAsEnum = EnumHelpers.GetEnumFromEnumDescription<TlsModes>(tlsMode);
                if (modeAsEnum == TlsModes.v10v11)
                {
                    list.Add(new TestGroup
                    {
                        HashAlg = new HashFunction(ModeValues.SHA1, DigestSizes.d160),
                        TlsMode = modeAsEnum,
                        KeyBlockLength = 832,
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
                            TlsMode = modeAsEnum,
                            KeyBlockLength = 1024,
                            PreMasterSecretLength = 384
                        });
                    }
                }
            }

            return Task.FromResult(list);
        }
    }
}
