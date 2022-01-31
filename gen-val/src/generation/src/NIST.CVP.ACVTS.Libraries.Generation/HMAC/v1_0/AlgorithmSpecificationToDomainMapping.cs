using System;
using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0
{
    public static class AlgorithmSpecificationToDomainMapping
    {
        public static List<(string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int minMacLength, int maxMacLength)> Mapping =
            new List<(string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int minMacLength, int maxMacLength)>()
        {
            ("HMAC-SHA-1", ModeValues.SHA1, DigestSizes.d160, 80, 160 ),
            ("HMAC-SHA2-224", ModeValues.SHA2, DigestSizes.d224, 112, 224 ),
            ("HMAC-SHA2-256", ModeValues.SHA2, DigestSizes.d256, 128, 256 ),
            ("HMAC-SHA2-384", ModeValues.SHA2, DigestSizes.d384, 192, 384 ),
            ("HMAC-SHA2-512", ModeValues.SHA2, DigestSizes.d512, 256, 512 ),
            ("HMAC-SHA2-512/224", ModeValues.SHA2, DigestSizes.d512t224, 112, 224 ),
            ("HMAC-SHA2-512/256", ModeValues.SHA2, DigestSizes.d512t256, 128, 256 ),
            ("HMAC-SHA3-224", ModeValues.SHA3, DigestSizes.d224, 112, 224 ),
            ("HMAC-SHA3-256", ModeValues.SHA3, DigestSizes.d256, 128, 256 ),
            ("HMAC-SHA3-384", ModeValues.SHA3, DigestSizes.d384, 192, 384 ),
            ("HMAC-SHA3-512", ModeValues.SHA3, DigestSizes.d512, 256, 512 ),
        };

        public static (string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int minMacLength, int maxMacLength) GetMappingFromAlgorithm(string algorithm)
        {
            if (!Mapping
                .TryFirst(
                    w => w.specificationAlgorithm.Equals(algorithm, StringComparison.OrdinalIgnoreCase),
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm provided.");
            }

            return result;
        }
    }
}
