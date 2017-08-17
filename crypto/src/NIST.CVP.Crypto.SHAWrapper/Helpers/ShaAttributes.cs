using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.SHAWrapper.Helpers
{
    /// <summary>
    /// Get SHA information
    /// </summary>
    public static class ShaAttributes
    {
        private static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize)> shaAttributes =
            new List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize)>()
            {
                (ModeValues.SHA1, DigestSizes.d160, 160, 512, ((BigInteger)1 << 64) - 1),
                (ModeValues.SHA2, DigestSizes.d224, 224, 512, ((BigInteger)1 << 64) - 1),
                (ModeValues.SHA2, DigestSizes.d256, 256, 512, ((BigInteger)1 << 64) - 1),
                (ModeValues.SHA2, DigestSizes.d384, 384, 1024, ((BigInteger)1 << 128) - 1),
                (ModeValues.SHA2, DigestSizes.d512, 512, 1024, ((BigInteger)1 << 128) - 1),
                (ModeValues.SHA2, DigestSizes.d512t224, 224, 1024, ((BigInteger)1 << 128) - 1),
                (ModeValues.SHA2, DigestSizes.d512t256, 256, 1024, ((BigInteger)1 << 128) - 1),
                (ModeValues.SHA3, DigestSizes.d224, 224, 1152, -1), // no limit
                (ModeValues.SHA3, DigestSizes.d256, 256, 1088, -1), // no limit
                (ModeValues.SHA3, DigestSizes.d384, 384, 832, -1), // no limit
                (ModeValues.SHA3, DigestSizes.d512, 512, 576, -1) // no limit
            };

        public static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize)> GetShaAttributes()
        {
            return shaAttributes;
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize, BigInteger maxMessageSize) GetShaAttributes(ModeValues mode, DigestSizes digestSize)
        {
            if (!GetShaAttributes()
                .TryFirst(w => w.mode == mode && w.digestSize == digestSize, out var result))
            {
                throw new ArgumentException($"Invalid {nameof(mode)}/{nameof(digestSize)} combination");
            }

            return result;
        }
    }
}
