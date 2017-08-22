using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    /// <inheritdoc />
    /// <summary>
    /// Key Confirmation parameters for both HMAC and CMAC
    /// </summary>
    public class KeyConfirmationParameters : IKeyConfirmationParameters
    {
        public KeyConfirmationParameters(
            KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationType keyConfirmationType,
            KeyConfirmationMacType macType, 
            int keyLength, 
            int macLength, 
            BitString thisPartyIdentifier, 
            BitString otherPartyIdentifier, 
            BitString thisPartyPublicKey, 
            BitString otherPartyPublicKey, 
            BitString derivedKeyingMaterial)
        {
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

        /// <inheritdoc />
        public KeyAgreementRole ThisPartyKeyAgreementRole { get; }
        /// <inheritdoc />
        public KeyConfirmationRole ThisPartyKeyConfirmationRole { get; }
        /// <inheritdoc />
        public KeyConfirmationType KeyConfirmationType { get; }
        /// <inheritdoc />
        public KeyConfirmationMacType MacType { get; }
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
    }
}
