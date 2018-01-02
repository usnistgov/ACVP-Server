using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.SHAWrapper.Helpers;
using NIST.CVP.Crypto.TLS.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TLS
{
    public class TestGroupGenerator : ITestGroupGenerator<Parameters>
    {
        public IEnumerable<ITestGroup> BuildTestGroups(Parameters parameters)
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

            return list;
        }
    }
}
