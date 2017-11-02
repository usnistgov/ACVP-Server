using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.KAS.Helpers
{
    public static class KeyGenerationRequirementsHelper
    {
        public static
            List<SchemeKeyNonceGenRequirement> FfcSchemeKeyGenerationRequirements =
                new List<SchemeKeyNonceGenRequirement>()
                {
                    new SchemeKeyNonceGenRequirement(FfcScheme.DhEphem, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        false, true, false),
                    new SchemeKeyNonceGenRequirement(FfcScheme.DhEphem, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        false, true, false),
                    new SchemeKeyNonceGenRequirement(FfcScheme.DhEphem, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        false, true, false),
                    new SchemeKeyNonceGenRequirement(FfcScheme.DhEphem, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        false, true, false)

                };
        
        public static SchemeKeyNonceGenRequirement GetKeyGenerationOptionsForSchemeAndRole(FfcScheme scheme, KasMode kasMode, KeyAgreementRole thisPartyRole, KeyConfirmationRole thisPartyKeyConfirmationRole, KeyConfirmationDirection keyConfirmationDirection)
        {
            if (!FfcSchemeKeyGenerationRequirements
                .TryFirst(w => 
                    w.Scheme == scheme &&
                    w.KasMode == kasMode &&
                    w.ThisPartyKasRole == thisPartyRole && 
                    w.ThisPartyKeyConfirmationRole == thisPartyKeyConfirmationRole &&
                    w.KeyConfirmationDirection == keyConfirmationDirection, out var result))
            {
                throw new ArgumentException("Invalid scheme/mode/key agreement role combination");
            }

            return result;
        }

        /// <summary>
        /// Gets the party B <see cref="KeyAgreementRole"/> from the <see cref="aPartyRole"/>
        /// </summary>
        /// <param name="aPartyRole">Party A's key agreement role</param>
        /// <returns>Party B's key agreement role</returns>
        public static KeyAgreementRole GetOtherPartyKeyAgreementRole(KeyAgreementRole aPartyRole)
        {
            if (aPartyRole == KeyAgreementRole.InitiatorPartyU)
            {
                return KeyAgreementRole.ResponderPartyV;
            }

            return KeyAgreementRole.InitiatorPartyU;
        }

        /// <summary>
        /// Gets the party B <see cref="KeyConfirmationRole"/> from the <see cref="aPartyRole"/>
        /// </summary>
        /// <param name="aPartyRole">Party A's key confirmation role</param>
        /// <returns>Party B's key confirmation role</returns>
        public static KeyConfirmationRole GetOtherPartyKeyConfirmationRole(KeyConfirmationRole aPartyRole)
        {
            switch (aPartyRole)
            {
                case KeyConfirmationRole.None:
                    return KeyConfirmationRole.None;
                case KeyConfirmationRole.Provider:
                    return KeyConfirmationRole.Recipient;
                case KeyConfirmationRole.Recipient:
                    return KeyConfirmationRole.Provider;
                default:
                    throw new ArgumentException();
            }
        }
    }
}