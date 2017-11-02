using System;
using System.Collections.Generic;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Generation.Core.ExtensionMethods;

namespace NIST.CVP.Crypto.KAS.Helpers
{
    public static class KeyGenerationRequirements
    {
        public static
            List<(FfcScheme scheme, KeyAgreementRole thisPartyKasRole, KasMode kasMode, bool generatesStaticKeyPair, bool generatesEphemeralKeyPair)>
            FfcSchemeKeyGenerationRequirements =
                new List<(FfcScheme scheme, KeyAgreementRole thisPartyKasRole, KasMode kasMode, bool generatesStaticKeyPair, bool generatesEphemeralKeyPair)>()
                {
                    (FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KasMode.NoKdfNoKc, false, true),
                    (FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KasMode.NoKdfNoKc, false, true),
                    (FfcScheme.DhEphem, KeyAgreementRole.InitiatorPartyU, KasMode.KdfNoKc, false, true),
                    (FfcScheme.DhEphem, KeyAgreementRole.ResponderPartyV, KasMode.KdfNoKc, false, true)

                };
        
        public static (FfcScheme scheme, KeyAgreementRole thisPartyKasRole, KasMode kasMode, bool generatesStaticKeyPair, bool generatesEphemeralKeyPair)
            GetKeyGenerationOptionsForSchemeAndRole(FfcScheme scheme, KeyAgreementRole thisPartyRole, KasMode kasMode)
        {
            if (!FfcSchemeKeyGenerationRequirements
                .TryFirst(w => 
                    w.scheme == scheme 
                    && w.thisPartyKasRole == thisPartyRole
                    && w.kasMode == kasMode, out var result))
            {
                throw new ArgumentException("Invalid scheme/key agreement role combination");
            }

            return result;
        }
    }
}