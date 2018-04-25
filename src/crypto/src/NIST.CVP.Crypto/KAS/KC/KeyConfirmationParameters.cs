using System;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    /// <inheritdoc />
    /// <summary>
    /// Key Confirmation parameters for both HMAC and CMAC
    /// </summary>
    public class KeyConfirmationParameters : IKeyConfirmationParameters
    {
        /// <summary>
        /// Non AES-CCM constructor
        /// </summary>
        /// <param name="thisPartyKeyAgreementRole"></param>
        /// <param name="thisPartyKeyConfirmationRole"></param>
        /// <param name="keyConfirmationType"></param>
        /// <param name="macType"></param>
        /// <param name="keyLength"></param>
        /// <param name="macLength"></param>
        /// <param name="thisPartyIdentifier"></param>
        /// <param name="otherPartyIdentifier"></param>
        /// <param name="thisPartyPublicKey"></param>
        /// <param name="otherPartyPublicKey"></param>
        /// <param name="derivedKeyingMaterial"></param>
        public KeyConfirmationParameters(
            KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationType,
            KeyAgreementMacType macType,
            int keyLength,
            int macLength,
            BitString thisPartyIdentifier,
            BitString otherPartyIdentifier,
            BitString thisPartyPublicKey,
            BitString otherPartyPublicKey,
            BitString derivedKeyingMaterial
        )
        {
            if (macType == KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }

            ThisPartyKeyAgreementRole = thisPartyKeyAgreementRole;
            ThisPartyKeyConfirmationRole = thisPartyKeyConfirmationRole;
            KeyConfirmationType = keyConfirmationType;
            MacType = macType;
            KeyLength = keyLength;
            MacLength = macLength;
            ThisPartyIdentifier = thisPartyIdentifier;
            OtherPartyIdentifier = otherPartyIdentifier;
            ThisPartyPublicKey = thisPartyPublicKey;
            OtherPartyPublicKey = otherPartyPublicKey;
            DerivedKeyingMaterial = derivedKeyingMaterial;
            KeyConfirmationType = keyConfirmationType;
        }

        /// <summary>
        /// AES-CCM constructor
        /// </summary>
        /// <param name="thisPartyKeyAgreementRole"></param>
        /// <param name="thisPartyKeyConfirmationRole"></param>
        /// <param name="keyConfirmationType"></param>
        /// <param name="macType"></param>
        /// <param name="keyLength"></param>
        /// <param name="macLength"></param>
        /// <param name="thisPartyIdentifier"></param>
        /// <param name="otherPartyIdentifier"></param>
        /// <param name="thisPartyPublicKey"></param>
        /// <param name="otherPartyPublicKey"></param>
        /// <param name="derivedKeyingMaterial"></param>
        /// <param name="ccmNonce"></param>
        public KeyConfirmationParameters(
            KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationType,
            KeyAgreementMacType macType, 
            int keyLength, 
            int macLength, 
            BitString thisPartyIdentifier, 
            BitString otherPartyIdentifier, 
            BitString thisPartyPublicKey, 
            BitString otherPartyPublicKey, 
            BitString derivedKeyingMaterial,
            BitString ccmNonce)
        {
            if (macType != KeyAgreementMacType.AesCcm)
            {
                throw new ArgumentException(nameof(macType));
            }
            if (BitString.IsZeroLengthOrNull(ccmNonce))
            {
                throw new ArgumentException(nameof(ccmNonce));
            }

            ThisPartyKeyAgreementRole = thisPartyKeyAgreementRole;
            ThisPartyKeyConfirmationRole = thisPartyKeyConfirmationRole;
            KeyConfirmationType = keyConfirmationType;
            MacType = macType;
            KeyLength = keyLength;
            MacLength = macLength;
            ThisPartyIdentifier = thisPartyIdentifier;
            OtherPartyIdentifier = otherPartyIdentifier;
            ThisPartyPublicKey = thisPartyPublicKey;
            OtherPartyPublicKey = otherPartyPublicKey;
            DerivedKeyingMaterial = derivedKeyingMaterial;
            KeyConfirmationType = keyConfirmationType;
            CcmNonce = ccmNonce;
        }

        /// <inheritdoc />
        public KeyAgreementRole ThisPartyKeyAgreementRole { get; }
        /// <inheritdoc />
        public KeyConfirmationRole ThisPartyKeyConfirmationRole { get; }
        /// <inheritdoc />
        public KeyConfirmationDirection KeyConfirmationType { get; }
        /// <inheritdoc />
        public KeyAgreementMacType MacType { get; }
        /// <inheritdoc />
        public int KeyLength { get; }
        /// <inheritdoc />
        public int MacLength { get; }
        /// <inheritdoc />
        public BitString ThisPartyIdentifier { get; }
        /// <inheritdoc />
        public BitString OtherPartyIdentifier { get; }
        /// <inheritdoc />
        public BitString ThisPartyPublicKey { get; }
        /// <inheritdoc />
        public BitString OtherPartyPublicKey { get; }
        /// <inheritdoc />
        public BitString DerivedKeyingMaterial { get; }
        /// <inheritdoc />
        public BitString CcmNonce { get; }
    }
}
