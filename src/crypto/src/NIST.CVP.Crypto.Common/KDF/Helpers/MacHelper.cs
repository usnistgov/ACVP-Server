using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Helpers;
using NIST.CVP.Crypto.Common.KDF.Enums;
using System;

namespace NIST.CVP.Crypto.Common.KDF.Helpers
{
    public static class MacHelper
    {
        public static int GetMacOutputLength(MacModes macMode)
        {
            switch (macMode)
            {
                case MacModes.CMAC_AES128:
                case MacModes.CMAC_AES192:
                case MacModes.CMAC_AES256:
                    return 128;
                case MacModes.CMAC_TDES:
                    return 64;
                case MacModes.HMAC_SHA1:
                    return ShaAttributes.GetShaAttributes(ModeValues.SHA1, DigestSizes.d160).outputLen;
                case MacModes.HMAC_SHA224:
                    return ShaAttributes.GetShaAttributes(ModeValues.SHA2, DigestSizes.d224).outputLen;
                case MacModes.HMAC_SHA256:
                    return ShaAttributes.GetShaAttributes(ModeValues.SHA2, DigestSizes.d256).outputLen;
                case MacModes.HMAC_SHA384:
                    return ShaAttributes.GetShaAttributes(ModeValues.SHA2, DigestSizes.d384).outputLen;
                case MacModes.HMAC_SHA512:
                    return ShaAttributes.GetShaAttributes(ModeValues.SHA2, DigestSizes.d512).outputLen;
            }

            throw new ArgumentException(nameof(macMode));
        }
    }
}