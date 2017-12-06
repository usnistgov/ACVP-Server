using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.KAS.Helpers
{
    public static class KeyGenerationRequirementsHelper
    {
        public static
            List<SchemeKeyNonceGenRequirement<FfcScheme>> FfcSchemeKeyGenerationRequirements =
                new List<SchemeKeyNonceGenRequirement<FfcScheme>>()
                {
                    #region dhEphem
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false, 
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    #endregion KdfNoKc
                    #endregion dhEphem

                    #region mqv1
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true
                    ),
                    #endregion KdfKc
                    #endregion mqv1
                };

        public static
            List<SchemeKeyNonceGenRequirement<EccScheme>> EccSchemeKeyGenerationRequirements =
                new List<SchemeKeyNonceGenRequirement<EccScheme>>()
                {
                    #region EphemeralUnified
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    #endregion KdfNoKc
                    #endregion EphemeralUnified

                    #region OnePassMqv
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true
                    ),
                    #endregion KdfKc
                    #endregion OnePassMqv
                };

        public static SchemeKeyNonceGenRequirement<FfcScheme> GetKeyGenerationOptionsForSchemeAndRole(
            FfcScheme scheme, 
            KasMode kasMode, 
            KeyAgreementRole thisPartyRole, 
            KeyConfirmationRole thisPartyKeyConfirmationRole, 
            KeyConfirmationDirection keyConfirmationDirection
        )
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

        public static SchemeKeyNonceGenRequirement<EccScheme> GetKeyGenerationOptionsForSchemeAndRole(
            EccScheme scheme,
            KasMode kasMode,
            KeyAgreementRole thisPartyRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection
        )
        {
            if (!EccSchemeKeyGenerationRequirements
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