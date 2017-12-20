using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS
{
    public static class SpecificationMapping
    {
        public static readonly BitString ServerId = new BitString("434156536964");
        public static readonly BitString IutId = new BitString("a1b2c3d4e5");

        #region hmac
        public static List<(string specificationHmac, Type macType, HashFunction hashFunction)> HmacMapping =
            new List<(string specificationHmac, Type macType, HashFunction hashFunction)>()
            {
                ("HMAC-SHA2-244", typeof(MacOptionHmacSha2_d224), new HashFunction(ModeValues.SHA2, DigestSizes.d224)),
                ("HMAC-SHA2-256", typeof(MacOptionHmacSha2_d256), new HashFunction(ModeValues.SHA2, DigestSizes.d256)),
                ("HMAC-SHA2-384", typeof(MacOptionHmacSha2_d384), new HashFunction(ModeValues.SHA2, DigestSizes.d384)),
                ("HMAC-SHA2-512", typeof(MacOptionHmacSha2_d512), new HashFunction(ModeValues.SHA2, DigestSizes.d512))
            };

        public static (string specificationHmac, Type macType, HashFunction hashFunction) GetHmacInfoFromParameterClass(MacOptionsBase macType)
        {
            if (!HmacMapping.TryFirst(w => w.macType.IsInstanceOfType(macType), out var result))
            {
                throw new ArgumentException(nameof(macType));
            }

            return result;
        }
        #endregion hmac

        #region mac
        public static List<(string specificationMac, Type macType, KeyAgreementMacType keyAgreementMacType)> MacMapping =
            new List<(string specificationMac, Type macType, KeyAgreementMacType keyAgreementMacType)>()
            {
                ("AES-CCM", typeof(MacOptionAesCcm), KeyAgreementMacType.AesCcm),
                ("CMAC", typeof(MacOptionCmac), KeyAgreementMacType.CmacAes),
                ("HMAC-SHA2-244", typeof(MacOptionHmacSha2_d224), KeyAgreementMacType.HmacSha2D224),
                ("HMAC-SHA2-256", typeof(MacOptionHmacSha2_d256), KeyAgreementMacType.HmacSha2D256),
                ("HMAC-SHA2-384", typeof(MacOptionHmacSha2_d384), KeyAgreementMacType.HmacSha2D384),
                ("HMAC-SHA2-512", typeof(MacOptionHmacSha2_d512), KeyAgreementMacType.HmacSha2D512)
            };

        public static (string specificationMac, Type macType, KeyAgreementMacType keyAgreementMacType) GetMacInfoFromParameterClass(MacOptionsBase macType)
        {
            if (!MacMapping.TryFirst(w => w.macType.IsInstanceOfType(macType), out var result))
            {
                throw new ArgumentException(nameof(macType));
            }

            return result;
        }
        #endregion mac

        #region scheme
        public static List<(FfcScheme schemeEnum, Type schemeParameter)> FfcSchemeMapping =
            new List<(FfcScheme schemeEnum, Type schemeParameter)>()
            {
                (FfcScheme.DhEphem, typeof(FfcDhEphem)),
                (FfcScheme.Mqv1, typeof(FfcMqv1)),
                (FfcScheme.DhHybrid1, typeof(FfcDhHybrid1)),
                (FfcScheme.DhHybridOneFlow, typeof(FfcDhHybridOneFlow)),
                (FfcScheme.DhOneFlow, typeof(FfcDhOneFlow)),
                (FfcScheme.Mqv2, typeof(FfcMqv2)),
                (FfcScheme.DhStatic, typeof(FfcDhStatic))
            };

        public static FfcScheme GetFfcEnumFromType(SchemeBase schemeBase)
        {
            if (!FfcSchemeMapping.TryFirst(w => w.schemeParameter.IsInstanceOfType(schemeBase), out var result))
            {
                throw new ArgumentException(nameof(schemeBase));
            }

            return result.schemeEnum;
        }

        public static List<(EccScheme schemeEnum, Type schemeParameter)> EccSchemeMapping =
            new List<(EccScheme schemeEnum, Type schemeParameter)>()
            {
                (EccScheme.EphemeralUnified, typeof(EccEphemeralUnified)),
                (EccScheme.OnePassMqv, typeof(EccOnePassMqv)),
                (EccScheme.FullMqv, typeof(EccFullMqv)),
                (EccScheme.FullUnified, typeof(EccFullUnified)),
                (EccScheme.OnePassDh, typeof(EccOnePassDh)),
                (EccScheme.OnePassUnified, typeof(EccOnePassUnified)),
                (EccScheme.StaticUnified, typeof(EccStaticUnified))
            };

        public static EccScheme GetEccEnumFromType(SchemeBase schemeBase)
        {
            if (!EccSchemeMapping.TryFirst(w => w.schemeParameter.IsInstanceOfType(schemeBase), out var result))
            {
                throw new ArgumentException(nameof(schemeBase));
            }

            return result.schemeEnum;
        }
        #endregion scheme

        #region functions
        public static KasAssurance FunctionArrayToFlags(string[] functions)
        {
            var flags = functions.Select(s => (KasAssurance)Enum.Parse(typeof(KasAssurance), s, true));

            KasAssurance assurance = KasAssurance.None;
            foreach (var flag in flags)
            {
                assurance |= flag;

                // Unset the none flag
                assurance &= ~KasAssurance.None;
            }

            return assurance;
        }
        #endregion functions
    }
}
