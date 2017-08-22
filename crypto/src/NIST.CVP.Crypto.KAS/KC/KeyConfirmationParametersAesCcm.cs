using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationParametersAesCcm : KeyConfirmationParameters
    {
        public KeyConfirmationParametersAesCcm(
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
            BitString derivedKeyingMaterial,
            BitString ccmNonce) : 
            base(thisPartyKeyAgreementRole, thisPartyKeyConfirmationRole, keyConfirmationType, macType, keyLength, macLength, thisPartyIdentifier, otherPartyIdentifier, thisPartyPublicKey, otherPartyPublicKey, derivedKeyingMaterial)
        {
            CcmNone = ccmNonce;
        }

        /// <summary>
        /// The nonce used for <see cref="IAES_CCM"/>
        /// </summary>
        public BitString CcmNone { get; }
    }
}
