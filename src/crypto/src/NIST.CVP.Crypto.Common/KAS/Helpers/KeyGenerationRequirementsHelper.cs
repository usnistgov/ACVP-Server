using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;

namespace NIST.CVP.Crypto.Common.KAS.Helpers
{
    public static class KeyGenerationRequirementsHelper
    {
        public static
            List<SchemeKeyNonceGenRequirement<FfcScheme>> FfcSchemeKeyGenerationRequirements =
                new List<SchemeKeyNonceGenRequirement<FfcScheme>>()
                {
                    #region dhHybrid1
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybrid1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion dhHybrid1

                    #region Mqv2
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv2, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion Mqv2

                    #region dhEphem
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false, 
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhEphem, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #endregion dhEphem

                    #region DhHybridOneFlow
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhHybridOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion DhHybridOneFlow

                    #region mqv1
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.Mqv1, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion mqv1

                    #region DhOneFlow
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhOneFlow, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhOneFlow, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhOneFlow, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhOneFlow, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    // this scheme allows only for unilateral key confirmation from V to U
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhOneFlow, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion DhOneFlow

                    #region DhStatic
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),

                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<FfcScheme>(
                        FfcScheme.DhStatic, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion DhStatic
                };

        public static
            List<SchemeKeyNonceGenRequirement<EccScheme>> EccSchemeKeyGenerationRequirements =
                new List<SchemeKeyNonceGenRequirement<EccScheme>>()
                {
                    #region FullUnified
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion FullUnified

                    #region FullMqv
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.FullMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion FullMqv

                    #region EphemeralUnified
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.EphemeralUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #endregion EphemeralUnified

                    #region OnePassUnified
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion OnePassUnified

                    #region OnePassMqv
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),

                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassMqv, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion OnePassMqv

                    #region OnePassDh
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassDh, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassDh, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassDh, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassDh, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    // this scheme allows only for unilateral key confirmation from V to U
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassDh, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: false,
                        generatesEphemeralKeyPair: true,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.OnePassDh, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion OnePassDh

                    #region StaticUnified
                    #region NoKdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.NoKdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion NoKdfNoKc
                    #region KdfNoKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfNoKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfNoKc
                    #region KdfKc
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: true
                    ),

                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: false,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    new SchemeKeyNonceGenRequirement<EccScheme>(
                        EccScheme.StaticUnified, KasMode.KdfKc,
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral,
                        generatesStaticKeyPair: true,
                        generatesEphemeralKeyPair: false,
                        generatesEphemeralNonce: true,
                        generatesDkmNonce: false
                    ),
                    #endregion KdfKc
                    #endregion StaticUnified
                };

        public static
            List<SchemeKeyNonceGenRequirement<IfcScheme>> IfcSchemeKeyGenerationRequirements =
                new List<SchemeKeyNonceGenRequirement<IfcScheme>>()
                {
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas1_basic, KasMode.KdfNoKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None, 
                        false, 
                        false
                        ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas1_basic, KasMode.KdfNoKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None, 
                        true, 
                        true
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas1_partyV_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, 
                        false, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas1_partyV_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, 
                        true, 
                        true
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_basic, KasMode.KdfNoKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_basic, KasMode.KdfNoKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_partyU_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_partyU_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_partyV_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_partyV_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_bilateral_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_bilateral_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_bilateral_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Provider, KeyConfirmationDirection.Bilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kas2_bilateral_keyConfirmation, KasMode.KdfKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Bilateral, 
                        true, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kts_oaep_basic, KasMode.NoKdfNoKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.None, KeyConfirmationDirection.None, 
                        false, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kts_oaep_basic, KasMode.NoKdfNoKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.None, KeyConfirmationDirection.None, 
                        true, 
                        true
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kts_oaep_partyV_keyConfirmation, KasMode.NoKdfKc, 
                        KeyAgreementRole.InitiatorPartyU, KeyConfirmationRole.Recipient, KeyConfirmationDirection.Unilateral, 
                        false, 
                        false
                    ),
                    new SchemeKeyNonceGenRequirement<IfcScheme>(
                        IfcScheme.Kts_oaep_partyV_keyConfirmation, KasMode.NoKdfKc, 
                        KeyAgreementRole.ResponderPartyV, KeyConfirmationRole.Provider, KeyConfirmationDirection.Unilateral, 
                        true, 
                        true
                    ),
                };
        
        public static SchemeKeyNonceGenRequirement GetKeyGenerationOptionsForSchemeAndRole(
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

        public static SchemeKeyNonceGenRequirement GetKeyGenerationOptionsForSchemeAndRole(
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
        
        public static SchemeKeyNonceGenRequirement GetKeyGenerationOptionsForSchemeAndRole(
            IfcScheme scheme,
            KasMode kasMode,
            KeyAgreementRole thisPartyRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection
        )
        {
            if (!IfcSchemeKeyGenerationRequirements
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

        public static readonly IfcScheme[] IfcKtsSchemes = 
        {
            IfcScheme.Kts_oaep_basic,
            IfcScheme.Kts_oaep_partyV_keyConfirmation
        };

        public static readonly IfcScheme[] IfcKdfSchemes =
        {
            IfcScheme.Kas1_basic,
            IfcScheme.Kas1_partyV_keyConfirmation,
            IfcScheme.Kas2_basic,
            IfcScheme.Kas2_bilateral_keyConfirmation,
            IfcScheme.Kas2_partyU_keyConfirmation,
            IfcScheme.Kas2_partyV_keyConfirmation
        };

        public static readonly IfcScheme[] IfcKcSchemes =
        {
            IfcScheme.Kas1_partyV_keyConfirmation,
            IfcScheme.Kas2_bilateral_keyConfirmation,
            IfcScheme.Kas2_partyU_keyConfirmation,
            IfcScheme.Kas2_partyV_keyConfirmation,
            IfcScheme.Kts_oaep_partyV_keyConfirmation
        };
        
        public static
            List<KeyConfirmationMacDetail> KeyConfirmationMacDetails =
                new List<KeyConfirmationMacDetail>()
                {
                    new KeyConfirmationMacDetail(KeyAgreementMacType.CmacAes, 128, 128, 256),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha2D224, 224, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha2D256, 256, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha2D384, 384, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha2D512, 512, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha2D512_T224, 224, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha2D512_T256, 256, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha3D224, 224, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha3D256, 256, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha3D384, 384, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.HmacSha3D512, 512, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.Kmac_128, 512, 112, 512),
                    new KeyConfirmationMacDetail(KeyAgreementMacType.Kmac_256, 512, 112, 512),
                };

        public static KeyConfirmationMacDetail GetKeyConfirmationMacDetails(KeyAgreementMacType macType)
        {
            if (!KeyConfirmationMacDetails.TryFirst(f => f.MacType == macType, out var result))
            {
                throw new ArgumentException($"Could not map {macType}");
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