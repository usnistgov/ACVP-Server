using NIST.CVP.Common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace NIST.CVP.Crypto.SHAWrapper.Helpers
{
    /// <summary>
    /// Get SHA information
    /// </summary>
    public static class ShaAttributes
    {
        private static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, string name)> shaAttributes =
            new List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, string name)>()
            {
                (ModeValues.SHA1, DigestSizes.d160, 160, 512, ((BigInteger)1 << 64) - 1, "sha-1"),
                (ModeValues.SHA2, DigestSizes.d224, 224, 512, ((BigInteger)1 << 64) - 1, "sha2-224"),
                (ModeValues.SHA2, DigestSizes.d256, 256, 512, ((BigInteger)1 << 64) - 1, "sha2-256"),
                (ModeValues.SHA2, DigestSizes.d384, 384, 1024, ((BigInteger)1 << 128) - 1, "sha2-384"),
                (ModeValues.SHA2, DigestSizes.d512, 512, 1024, ((BigInteger)1 << 128) - 1, "sha2-512"),
                (ModeValues.SHA2, DigestSizes.d512t224, 224, 1024, ((BigInteger)1 << 128) - 1, "sha2-512/224"),
                (ModeValues.SHA2, DigestSizes.d512t256, 256, 1024, ((BigInteger)1 << 128) - 1, "sha2-512/256"),
                (ModeValues.SHA3, DigestSizes.d224, 224, 1152, -1, "sha3-224"), // no limit
                (ModeValues.SHA3, DigestSizes.d256, 256, 1088, -1, "sha3-256"), // no limit
                (ModeValues.SHA3, DigestSizes.d384, 384, 832, -1, "sha3-384"), // no limit
                (ModeValues.SHA3, DigestSizes.d512, 512, 576, -1, "sha3-512") // no limit
            };

        public static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, string name)> GetShaAttributes()
        {
            return shaAttributes;
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, string name) GetShaAttributes(ModeValues mode, DigestSizes digestSize)
        {
            if (!GetShaAttributes()
                .TryFirst(w => w.mode == mode && w.digestSize == digestSize, out var result))
            {
                throw new ArgumentException($"Invalid {nameof(mode)}/{nameof(digestSize)} combination");
            }

            return result;
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize, string name) GetShaAttributes(string name)
        {
            if (!GetShaAttributes()
                .TryFirst(w => w.name.Equals(name, StringComparison.OrdinalIgnoreCase), out var result))
            {
                throw new ArgumentException($"Invalid sha {nameof(name)}");
            }

            return result;
        }

        public static HashFunction GetHashFunctionFromName(string name)
        {
            var attributes = GetShaAttributes(name);
            return new HashFunction(attributes.mode, attributes.digestSize);
        }
    }
}
