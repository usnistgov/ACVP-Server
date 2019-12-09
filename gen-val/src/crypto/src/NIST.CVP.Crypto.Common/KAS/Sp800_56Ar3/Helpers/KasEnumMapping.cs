using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Helpers
{
    public static class KasEnumMapping
    {
        public static Dictionary<EccScheme, KasScheme> EccMap = new Dictionary<EccScheme, KasScheme>()
        {
            {EccScheme.EphemeralUnified, KasScheme.EccEphemeralUnified},
            {EccScheme.FullMqv, KasScheme.EccFullMqv},
            {EccScheme.FullUnified, KasScheme.EccFullUnified},
            {EccScheme.StaticUnified, KasScheme.EccStaticUnified},
            {EccScheme.OnePassDh, KasScheme.EccOnePassDh},
            {EccScheme.OnePassMqv, KasScheme.EccOnePassMqv},
            {EccScheme.OnePassUnified, KasScheme.EccOnePassUnified},
            {EccScheme.None, KasScheme.None}
        };

        public static Dictionary<FfcScheme, KasScheme> FfcMap = new Dictionary<FfcScheme, KasScheme>()
        {
            {FfcScheme.Mqv1, KasScheme.FfcMqv1},
            {FfcScheme.Mqv2, KasScheme.FfcMqv2},
            {FfcScheme.DhEphem, KasScheme.FfcDhEphem},
            {FfcScheme.DhHybrid1, KasScheme.FfcDhHybrid1},
            {FfcScheme.DhStatic, KasScheme.FfcDhStatic},
            {FfcScheme.DhOneFlow, KasScheme.FfcDhOneFlow},
            {FfcScheme.DhHybridOneFlow, KasScheme.FfcDhHybridOneFlow},
            {FfcScheme.None, KasScheme.None}
        };
        
        public static (SchemeKeyNonceGenRequirement requirments, KasAlgorithm kasAlgo) GetSchemeRequirements(KasScheme scheme, KasMode kasMode, KeyAgreementRole thisPartyKeyAgreementRole, KeyConfirmationRole keyConfirmationRole, KeyConfirmationDirection keyConfirmationDirection)
        {
            KasEnumMapping.FfcMap.TryFirst(f => f.Value == scheme, out var ffcResult);
            KasEnumMapping.EccMap.TryFirst(f => f.Value == scheme, out var eccResult);

            if (ffcResult.Key == FfcScheme.None && eccResult.Key == EccScheme.None)
            {
                throw new ArgumentException($"Unable to map {nameof(scheme)} to {nameof(ffcResult)} or {nameof(eccResult)}");
            }

            if (ffcResult.Key != FfcScheme.None)
            {
                return (
                    KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                        ffcResult.Key, kasMode, thisPartyKeyAgreementRole, keyConfirmationRole, keyConfirmationDirection), 
                    KasAlgorithm.Ffc);
            }

            return (
                KeyGenerationRequirementsHelper.GetKeyGenerationOptionsForSchemeAndRole(
                    eccResult.Key, kasMode, thisPartyKeyAgreementRole, keyConfirmationRole, keyConfirmationDirection), 
                KasAlgorithm.Ecc);
        }
    }
}