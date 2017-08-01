using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Generation.Core.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.HMAC
{
    public static class AlgorithmSpecificationToDomainMapping
    {
        public static List<(string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int[] validMacLengths)> Mapping = 
            new List<(string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int[] validMacLengths)>()
        {
            ("HMAC-SHA-1", ModeValues.SHA1, DigestSizes.d160, new int[] { 80, 96, 128, 160 }),
            ("HMAC-SHA2-224", ModeValues.SHA2, DigestSizes.d224, new int[] { 112, 128, 160, 192, 224 }),
            ("HMAC-SHA2-256", ModeValues.SHA2, DigestSizes.d256, new int[] { 128, 192, 256 }),
            ("HMAC-SHA2-384", ModeValues.SHA2, DigestSizes.d384, new int[] { 192, 256, 320, 384 }),
            ("HMAC-SHA2-512", ModeValues.SHA2, DigestSizes.d512, new int[] { 256, 320, 384, 448, 512 }),
            ("HMAC-SHA2-512/224", ModeValues.SHA2, DigestSizes.d512t224, new int[] { 112, 128, 160, 192, 224 }),
            ("HMAC-SHA2-512/256", ModeValues.SHA2, DigestSizes.d512t256, new int[] { 128, 192, 256 }),
            ("HMAC-SHA3-224", ModeValues.SHA3, DigestSizes.d224, new int[] { 112, 128, 160, 192, 224 }),
            ("HMAC-SHA3-256", ModeValues.SHA3, DigestSizes.d256, new int[] { 128, 192, 256 }),
            ("HMAC-SHA3-384", ModeValues.SHA3, DigestSizes.d384, new int[] { 192, 256, 320, 384 }),
            ("HMAC-SHA3-512", ModeValues.SHA3, DigestSizes.d512, new int[] { 256, 320, 384, 448, 512 }),
        };

        public static (string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int[] validMacLengths) GetMappingFromAlgorithm(string algorithm)
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

        public static (string specificationAlgorithm, ModeValues shaMode, DigestSizes shaDigestSize, int[] validMacLengths) GetMappingFromAlgorithmOptions(ModeValues mode, DigestSizes digestSize)
        {
            if (!Mapping
                .TryFirst(
                    w => w.shaMode == mode && w.shaDigestSize == digestSize,
                    out var result))
            {
                throw new ArgumentException("Invalid Algorithm options.");
            }

            return result;
        }
    }
}
