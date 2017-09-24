using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Generation.KAS
{
    public static class SpecificationMapping
    {
        public static List<(string specificationHmac, Type macType, HashFunction hashFunction)> HmacMapping = 
            new List<(string specificationHmac, Type macType, HashFunction hashFunction)>()
        {
            ("HMAC-SHA2-244", typeof(MacOptionHmacSha2_d224), new HashFunction(ModeValues.SHA2, DigestSizes.d224)),
            ("HMAC-SHA2-256", typeof(MacOptionHmacSha2_d256), new HashFunction(ModeValues.SHA2, DigestSizes.d256)),
            ("HMAC-SHA2-384", typeof(MacOptionHmacSha2_d384), new HashFunction(ModeValues.SHA2, DigestSizes.d384)),
            ("HMAC-SHA2-512", typeof(MacOptionHmacSha2_d512), new HashFunction(ModeValues.SHA2, DigestSizes.d512)),
        };

        public static (string specificationHmac, Type macType, HashFunction hashFunction) GetHmacInfoFromParameterClass(
            MacOptionsBase macType)
        {
            if (!HmacMapping.TryFirst(w => w.macType.IsInstanceOfType(macType), out var result))
            {
                throw new ArgumentException(nameof(macType));
            }

            return result;
        }
    }
}
