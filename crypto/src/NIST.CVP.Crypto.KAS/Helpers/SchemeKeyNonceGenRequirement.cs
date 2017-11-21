using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.KAS.Enums;

namespace NIST.CVP.Crypto.KAS.Helpers
{
    public class SchemeKeyNonceGenRequirement<TScheme>
        where TScheme : struct, IComparable
    {
        public SchemeKeyNonceGenRequirement(TScheme scheme, KasMode kasMode, KeyAgreementRole thisPartyKasRole, KeyConfirmationRole thisPartyKeyConfirmationRole, KeyConfirmationDirection keyConfirmationDirection, bool generatesStaticKeyPair, bool generatesEphemeralKeyPair, bool generatesEphemeralNonce)
        {
            Scheme = scheme;
            KasMode = kasMode;
            ThisPartyKasRole = thisPartyKasRole;
            ThisPartyKeyConfirmationRole = thisPartyKeyConfirmationRole;
            KeyConfirmationDirection = keyConfirmationDirection;
            GeneratesStaticKeyPair = generatesStaticKeyPair;
            GeneratesEphemeralKeyPair = generatesEphemeralKeyPair;
            GeneratesEphemeralNonce = generatesEphemeralNonce;
        }

        public TScheme Scheme { get; }
        public KasMode KasMode { get; }
        public KeyAgreementRole ThisPartyKasRole { get; }
        public KeyConfirmationRole ThisPartyKeyConfirmationRole { get; }
        public KeyConfirmationDirection KeyConfirmationDirection { get; }
        public bool GeneratesStaticKeyPair { get; }
        public bool GeneratesEphemeralKeyPair { get; }
        public bool GeneratesEphemeralNonce { get; }
    }
}
