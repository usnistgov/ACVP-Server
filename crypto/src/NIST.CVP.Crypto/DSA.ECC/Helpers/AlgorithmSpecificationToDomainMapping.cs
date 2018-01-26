using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.DSA.ECC.Helpers
{
    public static class AlgorithmSpecificationToDomainMapping
    {
        public static List<(string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize)> Mapping =
            new List<(string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize)>()
        {
            ("SHA-1", ModeValues.SHA1, DigestSizes.d160),
            ("SHA2-224", ModeValues.SHA2, DigestSizes.d224),
            ("SHA2-256", ModeValues.SHA2, DigestSizes.d256),
            ("SHA2-384", ModeValues.SHA2, DigestSizes.d384),
            ("SHA2-512", ModeValues.SHA2, DigestSizes.d512),
            ("SHA2-512/224", ModeValues.SHA2, DigestSizes.d512t224),
            ("SHA2-512/256", ModeValues.SHA2, DigestSizes.d512t256),
            ("SHA3-224", ModeValues.SHA3, DigestSizes.d224),
            ("SHA3-256", ModeValues.SHA3, DigestSizes.d256),
            ("SHA3-384", ModeValues.SHA3, DigestSizes.d384),
            ("SHA3-512", ModeValues.SHA3, DigestSizes.d512),
        };

        public static (string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize) GetMappingFromAlgorithm(string algorithm)
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
