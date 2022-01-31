using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers
{
    public class SchemeKeyNonceGenRequirement<TScheme> : SchemeKeyNonceGenRequirement
        where TScheme : struct, IComparable
    {
        /// <summary>
        /// Constructor for FFC/ECC schemes.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="kasMode"></param>
        /// <param name="thisPartyKasRole"></param>
        /// <param name="thisPartyKeyConfirmationRole"></param>
        /// <param name="keyConfirmationDirection"></param>
        /// <param name="generatesStaticKeyPair"></param>
        /// <param name="generatesEphemeralKeyPair"></param>
        /// <param name="generatesEphemeralNonce"></param>
        /// <param name="generatesDkmNonce"></param>
        public SchemeKeyNonceGenRequirement(
            TScheme scheme,
            KasMode kasMode,
            KeyAgreementRole thisPartyKasRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            bool generatesStaticKeyPair,
            bool generatesEphemeralKeyPair,
            bool generatesEphemeralNonce,
            bool generatesDkmNonce
        ) : base(
            kasMode, thisPartyKasRole, thisPartyKeyConfirmationRole, keyConfirmationDirection, generatesStaticKeyPair, generatesEphemeralKeyPair, generatesEphemeralNonce, generatesDkmNonce)
        {
            Scheme = scheme;
        }

        /// <summary>
        /// Constructor for IFC schemes.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="kasMode"></param>
        /// <param name="thisPartyKasRole"></param>
        /// <param name="thisPartyKeyConfirmationRole"></param>
        /// <param name="keyConfirmationDirection"></param>
        /// <param name="generatesKeyPair"></param>
        /// <param name="generatesEphemeralNonce"></param>
        public SchemeKeyNonceGenRequirement(
            TScheme scheme,
            KasMode kasMode,
            KeyAgreementRole thisPartyKasRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            bool generatesKeyPair,
            bool generatesEphemeralNonce
        ) : base(
            kasMode, thisPartyKasRole, thisPartyKeyConfirmationRole, keyConfirmationDirection, generatesKeyPair, generatesEphemeralNonce)
        {
            Scheme = scheme;
        }

        public TScheme Scheme { get; }
    }


    public class SchemeKeyNonceGenRequirement
    {
        /// <summary>
        /// Constructor for FFC/ECC schemes.
        /// </summary>
        /// <param name="kasMode"></param>
        /// <param name="thisPartyKasRole"></param>
        /// <param name="thisPartyKeyConfirmationRole"></param>
        /// <param name="keyConfirmationDirection"></param>
        /// <param name="generatesStaticKeyPair"></param>
        /// <param name="generatesEphemeralKeyPair"></param>
        /// <param name="generatesEphemeralNonce"></param>
        /// <param name="generatesDkmNonce"></param>
        protected SchemeKeyNonceGenRequirement(
            KasMode kasMode,
            KeyAgreementRole thisPartyKasRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            bool generatesStaticKeyPair,
            bool generatesEphemeralKeyPair,
            bool generatesEphemeralNonce,
            bool generatesDkmNonce
        )
        {
            KasMode = kasMode;
            ThisPartyKasRole = thisPartyKasRole;
            ThisPartyKeyConfirmationRole = thisPartyKeyConfirmationRole;
            KeyConfirmationDirection = keyConfirmationDirection;
            GeneratesStaticKeyPair = generatesStaticKeyPair;
            GeneratesEphemeralKeyPair = generatesEphemeralKeyPair;
            GeneratesEphemeralNonce = generatesEphemeralNonce;
            GeneratesDkmNonce = generatesDkmNonce;
        }

        /// <summary>
        /// Constructor for IFC schemes. 
        /// </summary>
        /// <param name="kasMode"></param>
        /// <param name="thisPartyKasRole"></param>
        /// <param name="thisPartyKeyConfirmationRole"></param>
        /// <param name="keyConfirmationDirection"></param>
        /// <param name="generatesKeyPair"></param>
        /// <param name="generatesEphemeralNonce"></param>
        protected SchemeKeyNonceGenRequirement(
            KasMode kasMode,
            KeyAgreementRole thisPartyKasRole,
            KeyConfirmationRole thisPartyKeyConfirmationRole,
            KeyConfirmationDirection keyConfirmationDirection,
            bool generatesKeyPair,
            bool generatesEphemeralNonce)
        {
            KasMode = kasMode;
            ThisPartyKasRole = thisPartyKasRole;
            ThisPartyKeyConfirmationRole = thisPartyKeyConfirmationRole;
            KeyConfirmationDirection = keyConfirmationDirection;
            GeneratesStaticKeyPair = generatesKeyPair;
            GeneratesEphemeralKeyPair = generatesKeyPair;
            GeneratesEphemeralNonce = generatesEphemeralNonce;
        }

        public KasMode KasMode { get; }
        public KeyAgreementRole ThisPartyKasRole { get; }
        public KeyConfirmationRole ThisPartyKeyConfirmationRole { get; }
        public KeyConfirmationDirection KeyConfirmationDirection { get; }
        public bool GeneratesStaticKeyPair { get; }
        public bool GeneratesEphemeralKeyPair { get; }
        public bool GeneratesEphemeralNonce { get; }
        public bool GeneratesDkmNonce { get; }
    }
}
