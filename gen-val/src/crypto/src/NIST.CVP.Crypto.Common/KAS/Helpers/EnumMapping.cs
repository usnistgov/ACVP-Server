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
            switch (keyAgreementMacType)
            {
                case KeyAgreementMacType.HmacSha2D224:
                    modeValue = ModeValues.SHA2;
                    digestSize = DigestSizes.d224;
                    break;
                case KeyAgreementMacType.HmacSha2D256:
                    modeValue = ModeValues.SHA2;
                    digestSize = DigestSizes.d256;
                    break;
                case KeyAgreementMacType.HmacSha2D384:
                    modeValue = ModeValues.SHA2;
                    digestSize = DigestSizes.d384;
                    break;
                case KeyAgreementMacType.HmacSha2D512:
                    modeValue = ModeValues.SHA2;
                    digestSize = DigestSizes.d512;
                    break;
                case KeyAgreementMacType.HmacSha2D512_T224:
                    modeValue = ModeValues.SHA2;
                    digestSize = DigestSizes.d512t224;
                    break;
                case KeyAgreementMacType.HmacSha2D512_T256:
                    modeValue = ModeValues.SHA2;
                    digestSize = DigestSizes.d512t256;
                    break;
                case KeyAgreementMacType.HmacSha3D224:
                    modeValue = ModeValues.SHA3;
                    digestSize = DigestSizes.d224;
                    break;
                case KeyAgreementMacType.HmacSha3D256:
                    modeValue = ModeValues.SHA3;
                    digestSize = DigestSizes.d256;
                    break;
                case KeyAgreementMacType.HmacSha3D384:
                    modeValue = ModeValues.SHA3;
                    digestSize = DigestSizes.d384;
                    break;
                case KeyAgreementMacType.HmacSha3D512:
                    modeValue = ModeValues.SHA3;
                    digestSize = DigestSizes.d512;
                    break;
                default:
                    throw new ArgumentException($"{typeof(EnumMapping)}, {nameof(keyAgreementMacType)}");
            }
        }
    }
}
