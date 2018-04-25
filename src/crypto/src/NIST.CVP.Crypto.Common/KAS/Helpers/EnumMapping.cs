using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Helpers
{
    public static class EnumMapping
    {
        /// <summary>
        /// Determines the correct <see cref="ModeValues"/> and <see cref="DigestSizes"/> based on <see cref="KeyAgreementMacType"/>
        /// </summary>
        /// <param name="keyAgreementMacType">The key agreement mac type to check against</param>
        /// <param name="modeValue">The equivalent mode</param>
        /// <param name="digestSize">The equivalent digest size</param>
        public static void GetHashFunctionOptions(KeyAgreementMacType keyAgreementMacType, ref ModeValues modeValue, ref DigestSizes digestSize)
        {
            // Always the case (at least for now)
            modeValue = ModeValues.SHA2;

            switch (keyAgreementMacType)
            {
                case KeyAgreementMacType.HmacSha2D224:
                    digestSize = DigestSizes.d224;
                    break;
                case KeyAgreementMacType.HmacSha2D256:
                    digestSize = DigestSizes.d256;
                    break;
                case KeyAgreementMacType.HmacSha2D384:
                    digestSize = DigestSizes.d384;
                    break;
                case KeyAgreementMacType.HmacSha2D512:
                    digestSize = DigestSizes.d512;
                    break;
                default:
                    throw new ArgumentException(nameof(keyAgreementMacType));
            }
        }
    }
}
