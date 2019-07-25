using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationMacDataCreator : IKeyConfirmationMacDataCreator
    {
        private static readonly List<(string message, bool thisPartyInfoFirst, KeyAgreementRole thisPartyKeyAgreementRole, KeyConfirmationRole
            thisPartyKeyConfirmationRole, KeyConfirmationDirection keyConfirmationType)> _messageMapping =
            new List<(string message, bool thisPartyInfoFirst, KeyAgreementRole thisPartyKeyAgreementRole, KeyConfirmationRole
                thisPartyKeyConfirmationRole, KeyConfirmationDirection keyConfirmationType)>()
            {
                ("KC_1_U", true, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral),
                ("KC_1_V", false, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral),
                ("KC_1_V", true, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral),
                ("KC_1_U", false, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral),
                ("KC_2_U", true, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral),
                ("KC_2_V", false, KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral),
                ("KC_2_V", true, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral),
                ("KC_2_U", false, KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral),
            };
        
        public BitString GetMacData(IKeyConfirmationParameters param)
        {
            // Depending on options, macData made up of:
            // MAC Data = message||idU||idV||ephemU||ephemV{||Text}
            // MAC Data = message||idV||idU||ephemV||ephemU{||Text}

            bool thisPartyInfoFirst = false;
            var message = SetMessageStringFromOptions(
                param.ThisPartyKeyAgreementRole,
                param.ThisPartyKeyConfirmationRole, 
                param.KeyConfirmationType,
                ref thisPartyInfoFirst
            );

            // Guard against null keys (can happen for some schemes)
            var thisPartyPublicKey = ZeroLengthKeyWhenNullOtherwiseKey(param.ThisPartyPublicKey);
            var otherPartyPublicKey = ZeroLengthKeyWhenNullOtherwiseKey(param.OtherPartyPublicKey);

            if (thisPartyInfoFirst)
            {
                return message
                    .ConcatenateBits(param.ThisPartyIdentifier)
                    .ConcatenateBits(param.OtherPartyIdentifier)
                    .ConcatenateBits(thisPartyPublicKey)
                    .ConcatenateBits(otherPartyPublicKey);
            }

            return message
                .ConcatenateBits(param.OtherPartyIdentifier)
                .ConcatenateBits(param.ThisPartyIdentifier)
                .ConcatenateBits(otherPartyPublicKey)
                .ConcatenateBits(thisPartyPublicKey);
        }

        private BitString SetMessageStringFromOptions(
            KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationType,
            ref bool thisPartyInfoFirst
        )
        {
            if (!_messageMapping.TryFirst(w =>
                    w.thisPartyKeyAgreementRole == thisPartyKeyAgreementRole &&
                    w.thisPartyKeyConfirmationRole == thisPartyKeyConfirmationRole &&
                    w.keyConfirmationType == keyConfirmationType
                , out var result))
            {
                throw new ArgumentException("Unable to determine MACDataMessage");
            }

            thisPartyInfoFirst = result.thisPartyInfoFirst;
            
            return new BitString(Encoding.ASCII.GetBytes(result.message));
        }

        /// <summary>
        /// Returns empty bitstring in instances where null bitstring encountered.
        /// </summary>
        /// <param name="keyToCheck"></param>
        /// <returns></returns>
        private BitString ZeroLengthKeyWhenNullOtherwiseKey(BitString keyToCheck)
        {
            if (keyToCheck == null)
            {
                return new BitString(0);
            }

            return keyToCheck;
        }
    }
}