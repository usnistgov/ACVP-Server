using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
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
            FfcMap.TryFirst(f => f.Value == scheme, out var ffcResult);
            EccMap.TryFirst(f => f.Value == scheme, out var eccResult);

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

        public static Curve GetCurveFromKasDpGeneration(KasDpGeneration dpGeneration)
        {
            return dpGeneration switch
            {
                KasDpGeneration.P192 => Curve.P192,
                KasDpGeneration.P224 => Curve.P224,
                KasDpGeneration.P256 => Curve.P256,
                KasDpGeneration.P384 => Curve.P384,
                KasDpGeneration.P521 => Curve.P521,
                KasDpGeneration.K163 => Curve.K163,
                KasDpGeneration.K233 => Curve.K233,
                KasDpGeneration.K283 => Curve.K283,
                KasDpGeneration.K409 => Curve.K409,
                KasDpGeneration.K571 => Curve.K571,
                KasDpGeneration.B163 => Curve.B163,
                KasDpGeneration.B233 => Curve.B233,
                KasDpGeneration.B283 => Curve.B283,
                KasDpGeneration.B409 => Curve.B409,
                KasDpGeneration.B571 => Curve.B571,
                _ => throw new ArgumentException(nameof(dpGeneration))
            };
        }
    }
}