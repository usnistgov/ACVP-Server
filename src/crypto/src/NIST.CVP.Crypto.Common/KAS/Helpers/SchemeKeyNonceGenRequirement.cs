using System;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Helpers
{
    public class SchemeKeyNonceGenRequirement<TScheme> : SchemeKeyNonceGenRequirement
        where TScheme : struct, IComparable
    {
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
        ) : base (
            kasMode, thisPartyKasRole, thisPartyKeyConfirmationRole, keyConfirmationDirection,generatesStaticKeyPair, generatesEphemeralKeyPair, generatesEphemeralNonce, generatesDkmNonce)
        {
            Scheme = scheme;
        }

        public TScheme Scheme { get; }
    }

    public class SchemeKeyNonceGenRequirement
    {
        public SchemeKeyNonceGenRequirement(
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
