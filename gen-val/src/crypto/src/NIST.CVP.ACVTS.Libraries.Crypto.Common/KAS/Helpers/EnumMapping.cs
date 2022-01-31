using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers
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
                case KeyAgreementMacType.HmacSha1:
                    modeValue = ModeValues.SHA1;
                    digestSize = DigestSizes.d160;
                    break;
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

        /// <summary>
        /// Get <see cref="HashFunctions"/> from the provided <see cref="KasHashAlg"/>.
        /// </summary>
        /// <param name="kasHashAlg">The <see cref="KasHashAlg"/> to use for getting a <see cref="HashFunctions"/>.</param>
        /// <returns></returns>
        public static HashFunctions GetHashFunctionEnumFromKasHashFunctionEnum(KasHashAlg kasHashAlg)
        {
            switch (kasHashAlg)
            {
                case KasHashAlg.SHA1:
                    return HashFunctions.Sha1;
                case KasHashAlg.SHA2_D224:
                    return HashFunctions.Sha2_d224;
                case KasHashAlg.SHA2_D256:
                    return HashFunctions.Sha2_d256;
                case KasHashAlg.SHA2_D384:
                    return HashFunctions.Sha2_d384;
                case KasHashAlg.SHA2_D512:
                    return HashFunctions.Sha2_d512;
                case KasHashAlg.SHA2_D512_T224:
                    return HashFunctions.Sha2_d512t224;
                case KasHashAlg.SHA2_D512_T256:
                    return HashFunctions.Sha2_d512t256;
                case KasHashAlg.SHA3_D224:
                    return HashFunctions.Sha3_d224;
                case KasHashAlg.SHA3_D256:
                    return HashFunctions.Sha3_d256;
                case KasHashAlg.SHA3_D384:
                    return HashFunctions.Sha3_d384;
                case KasHashAlg.SHA3_D512:
                    return HashFunctions.Sha3_d512;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kasHashAlg), kasHashAlg, null);
            }
        }

        public static int GetMaxOutputLengthOfDkmForOneStepAuxFunction(KdaOneStepAuxFunction auxFunction)
        {
            switch (auxFunction)
            {
                case KdaOneStepAuxFunction.SHA1:
                case KdaOneStepAuxFunction.HMAC_SHA1:
                    return 160;
                case KdaOneStepAuxFunction.SHA2_D224:
                case KdaOneStepAuxFunction.SHA2_D512_T224:
                case KdaOneStepAuxFunction.SHA3_D224:
                case KdaOneStepAuxFunction.HMAC_SHA2_D224:
                case KdaOneStepAuxFunction.HMAC_SHA2_D512_T224:
                case KdaOneStepAuxFunction.HMAC_SHA3_D224:
                    return 224;
                case KdaOneStepAuxFunction.SHA2_D256:
                case KdaOneStepAuxFunction.SHA2_D512_T256:
                case KdaOneStepAuxFunction.SHA3_D256:
                case KdaOneStepAuxFunction.HMAC_SHA2_D256:
                case KdaOneStepAuxFunction.HMAC_SHA2_D512_T256:
                case KdaOneStepAuxFunction.HMAC_SHA3_D256:
                    return 256;
                case KdaOneStepAuxFunction.SHA2_D384:
                case KdaOneStepAuxFunction.SHA3_D384:
                case KdaOneStepAuxFunction.HMAC_SHA2_D384:
                case KdaOneStepAuxFunction.HMAC_SHA3_D384:
                    return 384;
                case KdaOneStepAuxFunction.SHA2_D512:
                case KdaOneStepAuxFunction.SHA3_D512:
                case KdaOneStepAuxFunction.HMAC_SHA2_D512:
                case KdaOneStepAuxFunction.HMAC_SHA3_D512:
                    return 512;
                case KdaOneStepAuxFunction.KMAC_128:
                case KdaOneStepAuxFunction.KMAC_256:
                    return 2048; // arbitrary, but we don't want to do a DKM exceeding this for testing 
                default:
                    throw new ArgumentOutOfRangeException(nameof(auxFunction), auxFunction, null);
            }
        }
    }
}
