using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationParameters : INoKeyConfirmationParameters
    {
        /// <summary>
        /// Constructor used for AES-CCM NoKeyConfirmation
        /// </summary>
        /// <param name="keyAgreementMacType">The MAC type used</param>
        /// <param name="derivedKeyingMaterial">The keying material used as the MAC key</param>
        /// <param name="macLength">The returned MAC length</param>
        /// <param name="nonce">The nonce used (concatenated onto "Standard Test Message")</param>
        /// <param name="ccmNonce">The Nonce used in <see cref="IAES_CCM"/> nonce</param>
        public NoKeyConfirmationParameters(KeyAgreementMacType keyAgreementMacType, int macLength, BitString derivedKeyingMaterial, BitString nonce, BitString ccmNonce)
        {
            if (keyAgreementMacType != KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(keyAgreementMacType));
            }

            if (BitString.IsZeroLengthOrNull(ccmNonce))
            {
                throw new ArgumentException(nameof(ccmNonce));
            }

            KeyAgreementMacType = keyAgreementMacType;
            MacLength = macLength;
            DerivedKeyingMaterial = derivedKeyingMaterial;
            Nonce = nonce;
            CcmNonce = ccmNonce;
        }

        /// <summary>
        /// Constructor used for MACs (not AES-CCM)
        /// </summary>
        /// <param name="keyAgreementMacType">The MAC type used</param>
        /// <param name="derivedKeyingMaterial">The keying material used as the MAC key</param>
        /// <param name="macLength">The returned MAC length</param>
        /// <param name="nonce">The nonce used (concatenated onto "Standard Test Message")</param>
        public NoKeyConfirmationParameters(KeyAgreementMacType keyAgreementMacType, int macLength, BitString derivedKeyingMaterial, BitString nonce)
        {
            if (keyAgreementMacType == KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(keyAgreementMacType));
            }

            KeyAgreementMacType = keyAgreementMacType;
            MacLength = macLength;
            DerivedKeyingMaterial = derivedKeyingMaterial;
            Nonce = nonce;
        }

        /// <inheritdoc />
        public KeyAgreementMacType KeyAgreementMacType { get; }
        /// <inheritdoc />
        public int MacLength { get; }
        /// <inheritdoc />
        public BitString DerivedKeyingMaterial { get; }
        /// <inheritdoc />
        public BitString Nonce { get; }
        /// <inheritdoc />
        public BitString CcmNonce { get; }
    }
}
