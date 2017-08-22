using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;
using NIST.CVP.Math;
using NLog.LayoutRenderers;

namespace NIST.CVP.Crypto.KAS.KC
{
    public abstract class KeyConfirmationBase : IKeyConfirmation
    {
        private static
            List<(string message, bool thisPartyInfoFirst, KeyAgreementRole thisPartyKeyAgreementRole, KeyConfirmationRole
                thisPartyKeyConfirmationRole, KeyConfirmationType keyConfirmationType)> _messageMapping =
                new List<(string message, bool thisPartyInfoFirst, KeyAgreementRole thisPartyKeyAgreementRole, KeyConfirmationRole
                    thisPartyKeyConfirmationRole, KeyConfirmationType keyConfirmationType)>()
                {
                    ("KC_1_U", true, KeyAgreementRole.UPartyInitiator, KeyConfirmationRole.Provider, KeyConfirmationType.Unilateral),
                    ("KC_1_V", false, KeyAgreementRole.UPartyInitiator, KeyConfirmationRole.Recipient, KeyConfirmationType.Unilateral),
                    ("KC_1_V", true, KeyAgreementRole.VPartyResponder, KeyConfirmationRole.Provider, KeyConfirmationType.Unilateral),
                    ("KC_1_U", false, KeyAgreementRole.VPartyResponder, KeyConfirmationRole.Recipient, KeyConfirmationType.Unilateral),
                    ("KC_2_U", true, KeyAgreementRole.UPartyInitiator, KeyConfirmationRole.Provider, KeyConfirmationType.Bilateral),
                    ("KC_2_V", false, KeyAgreementRole.UPartyInitiator, KeyConfirmationRole.Recipient, KeyConfirmationType.Bilateral),
                    ("KC_2_V", true, KeyAgreementRole.VPartyResponder, KeyConfirmationRole.Provider, KeyConfirmationType.Bilateral),
                    ("KC_2_U", false, KeyAgreementRole.VPartyResponder, KeyConfirmationRole.Recipient, KeyConfirmationType.Bilateral),
                };
        
        public abstract ComputeKeyResult ComputeKeyMac(IKeyConfirmationParameters keyConfirmationParameters);

        protected BitString GenerateMacData(IKeyConfirmationParameters keyConfirmationParameters)
        {
            // Depending on options, macData made up of:
            // MAC Data = message||idU||idV||ephemU||ephemV{||Text}
            // MAC Data = message||idV||idU||ephemV||ephemU{||Text}

            bool thisPartyInfoFirst = false;
            var message = SetMessageStringFromOptions(
                keyConfirmationParameters.ThisPartyKeyAgreementRole,
                keyConfirmationParameters.ThisPartyKeyConfirmationRole, 
                keyConfirmationParameters.KeyConfirmationType,
                ref thisPartyInfoFirst
            );

            // Guard against null keys (can happen for some schemes)
            var thisPartyPublicKey = ZeroLengthKeyWhenNullOtherwiseKey(keyConfirmationParameters.ThisPartyPublicKey);
            var otherPartyPublicKey = ZeroLengthKeyWhenNullOtherwiseKey(keyConfirmationParameters.OtherPartyPublicKey);

            if (thisPartyInfoFirst)
            {
                return message
                    .ConcatenateBits(keyConfirmationParameters.ThisPartyIdentifier)
                    .ConcatenateBits(keyConfirmationParameters.OtherPartyIdentifier)
                    .ConcatenateBits(thisPartyPublicKey)
                    .ConcatenateBits(otherPartyPublicKey);
            }

            return message
                .ConcatenateBits(keyConfirmationParameters.OtherPartyIdentifier)
                .ConcatenateBits(keyConfirmationParameters.ThisPartyIdentifier)
                .ConcatenateBits(otherPartyPublicKey)
                .ConcatenateBits(thisPartyPublicKey);
        }

        private BitString SetMessageStringFromOptions(
            KeyAgreementRole thisPartyKeyAgreementRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole, 
            KeyConfirmationType keyConfirmationType,
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
