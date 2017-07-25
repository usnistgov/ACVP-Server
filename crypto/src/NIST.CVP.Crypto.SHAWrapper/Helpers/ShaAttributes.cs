using System;
using System.Collections.Generic;
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
        private static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize)> shaAttributes =
            new List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize)>()
            {
                (ModeValues.SHA1, DigestSizes.d160, 160, 512),
                (ModeValues.SHA2, DigestSizes.d224, 224, 512),
                (ModeValues.SHA2, DigestSizes.d256, 256, 512),
                (ModeValues.SHA2, DigestSizes.d384, 384, 1024),
                (ModeValues.SHA2, DigestSizes.d512, 512, 1024),
                (ModeValues.SHA2, DigestSizes.d512t224, 224, 1024),
                (ModeValues.SHA2, DigestSizes.d512t256, 256, 1024),
                (ModeValues.SHA3, DigestSizes.d224, 224, 1152),
                (ModeValues.SHA3, DigestSizes.d256, 256, 1088),
                (ModeValues.SHA3, DigestSizes.d384, 384, 832),
                (ModeValues.SHA3, DigestSizes.d512, 512, 576)
            };

        public static List<(ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize)> GetShaAttributes()
        {
            return shaAttributes;
        }

        public static (ModeValues mode, DigestSizes digestSize, int outputLen, int blockSize) GetShaAttributes(ModeValues mode, DigestSizes digestSize)
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
