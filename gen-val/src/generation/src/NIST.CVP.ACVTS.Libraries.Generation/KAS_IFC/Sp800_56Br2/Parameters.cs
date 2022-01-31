using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Generation.Core;
using NIST.CVP.ACVTS.Libraries.Generation.KDF.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_IFC.Sp800_56Br2
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public BitString IutId { get; set; }
        public string[] Conformances { get; set; }

        /// <summary>
        /// KAS Assurances
        /// </summary>
        public string[] Function { get; set; }

        /// <summary>
        /// The KAS schemes to test
        /// </summary>
        public Schemes Scheme { get; set; }
        /// <summary>
        /// The key generation methods to test.
        /// </summary>
        public IfcKeyGenerationMethod[] KeyGenerationMethods { get; set; }
        /// <summary>
        /// The modulo lengths supported.
        /// </summary>
        public int[] Modulo { get; set; }
        /// <summary>
        /// The fixed public exponent (only applicable for rsakpg1*).
        /// </summary>
        [JsonProperty(PropertyName = "fixedPubExp", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BigInteger PublicExponent { get; set; }
    }

    public class Schemes
    {
        [JsonProperty(PropertyName = "KAS1-basic")]
        public Kas1_basic Kas1_basic { get; set; }
        [JsonProperty(PropertyName = "KAS1-Party_V-confirmation")]
        public Kas1_partyV_confirmation Kas1_partyV_confirmation { get; set; }
        [JsonProperty(PropertyName = "KAS2-basic")]
        public Kas2_basic Kas2_basic { get; set; }
        [JsonProperty(PropertyName = "KAS2-bilateral-confirmation")]
        public Kas2_bilateral_confirmation Kas2_bilateral_confirmation { get; set; }
        [JsonProperty(PropertyName = "KAS2-Party_U-confirmation")]
        public Kas2_partyU_confirmation Kas2_partyU_confirmation { get; set; }
        [JsonProperty(PropertyName = "KAS2-Party_V-confirmation")]
        public Kas2_partyV_confirmation Kas2_partyV_confirmation { get; set; }
        [JsonProperty(PropertyName = "KTS-OAEP-basic")]
        public Kts_oaep_basic Kts_oaep_basic { get; set; }
        [JsonProperty(PropertyName = "KTS-OAEP-Party_V-confirmation")]
        public Kts_oaep_partyV_confirmation Kts_oaep_partyV_confirmation { get; set; }

        public IEnumerable<SchemeBase> GetRegisteredSchemes()
        {
            var list = new List<SchemeBase>();

            list.AddIfNotNull(Kas1_basic);
            list.AddIfNotNull(Kas1_partyV_confirmation);
            list.AddIfNotNull(Kas2_basic);
            list.AddIfNotNull(Kas2_bilateral_confirmation);
            list.AddIfNotNull(Kas2_partyU_confirmation);
            list.AddIfNotNull(Kas2_partyV_confirmation);
            list.AddIfNotNull(Kts_oaep_basic);
            list.AddIfNotNull(Kts_oaep_partyV_confirmation);

            return list;
        }
    }

    public abstract class SchemeBase
    {
        /// <summary>
        /// The enum scheme type
        /// </summary>
        public abstract IfcScheme Scheme { get; }
        /// <summary>
        /// Additional operations the scheme supports.
        /// </summary>
        public abstract KasMode KasMode { get; }
        /// <summary>
        /// The Key Agreement Role (initiator or responder)
        /// </summary>
        public KeyAgreementRole[] KasRole { get; set; }
        /// <summary>
        /// The KDF types used under this scheme (KAS schemes only)
        /// </summary>
        public KdfMethods KdfMethods { get; set; }
        /// <summary>
        /// The KTS capabilities used (KTS schemes only)
        /// </summary>
        public KtsMethod KtsMethod { get; set; }
        /// <summary>
        /// The MAC used for schemes utilizing KeyConfirmation
        /// </summary>
        public MacMethods MacMethods { get; set; }
        /// <summary>
        /// The length of the key to derive (using a KDF) or transport (using a KTS scheme).
        /// </summary>
        /// <remarks>This value should be large enough to accommodate the maximum key length used for a mac algorithm.</remarks>
        public int L { get; set; }
    }

    public class Kas1_basic : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kas1_basic;
        public override KasMode KasMode => KasMode.KdfNoKc;
    }

    public class Kas1_partyV_confirmation : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kas1_partyV_keyConfirmation;
        public override KasMode KasMode => KasMode.KdfKc;
    }

    public class Kas2_basic : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kas2_basic;
        public override KasMode KasMode => KasMode.KdfNoKc;
    }

    public class Kas2_bilateral_confirmation : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kas2_bilateral_keyConfirmation;
        public override KasMode KasMode => KasMode.KdfKc;
    }

    public class Kas2_partyU_confirmation : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kas2_partyU_keyConfirmation;
        public override KasMode KasMode => KasMode.KdfKc;
    }

    public class Kas2_partyV_confirmation : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kas2_partyV_keyConfirmation;
        public override KasMode KasMode => KasMode.KdfKc;
    }

    public class Kts_oaep_basic : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kts_oaep_basic;
        public override KasMode KasMode => KasMode.NoKdfNoKc;
    }

    public class Kts_oaep_partyV_confirmation : SchemeBase
    {
        public override IfcScheme Scheme => IfcScheme.Kts_oaep_partyV_keyConfirmation;
        public override KasMode KasMode => KasMode.NoKdfKc;
    }

    public class KdfMethods
    {
        public OneStepKdf OneStepKdf { get; set; }
        public OneStepNoCounterKdf OneStepNoCounterKdf { get; set; }
        public TwoStepKdf TwoStepKdf { get; set; }
        public IkeV1Kdf IkeV1Kdf { get; set; }
        public IkeV2Kdf IkeV2Kdf { get; set; }
        public TlsV10_11Kdf TlsV10_11Kdf { get; set; }
        public TlsV12Kdf TlsV12Kdf { get; set; }

        public IEnumerable<KdfMethodBase> GetRegisteredKdfMethods()
        {
            var list = new List<KdfMethodBase>();

            list.AddIfNotNull(OneStepKdf);
            list.AddIfNotNull(OneStepNoCounterKdf);
            list.AddIfNotNull(TwoStepKdf);

            // Application specific KDFs removed due to a meeting with the CT group 2019/10/22
            //list.AddIfNotNull(IkeV1Kdf);
            //list.AddIfNotNull(IkeV2Kdf);
            //list.AddIfNotNull(TlsV10_11Kdf);
            //list.AddIfNotNull(TlsV12Kdf);

            return list;
        }
    }

    public abstract class KdfMethodBase
    {
        /// <summary>
        /// The enum type for the KDF
        /// </summary>
        public abstract Kda KdfType { get; }
    }

    public class OneStepKdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.OneStep;
        /// <summary>
        /// The Hash or MAC functions utilized for the KDF
        /// </summary>
        public AuxFunction[] AuxFunctions { get; set; }
        /// <summary>
        /// The pattern used for FixedInputConstruction.
        /// </summary>
        public string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        public FixedInfoEncoding[] Encoding { get; set; }
    }

    public class OneStepNoCounterKdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.OneStepNoCounter;
        /// <summary>
        /// The Hash or MAC functions utilized for the KDF
        /// </summary>
        public AuxFunctionNoCounter[] AuxFunctions { get; set; }
        /// <summary>
        /// The pattern used for FixedInputConstruction.
        /// </summary>
        public string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        public FixedInfoEncoding[] Encoding { get; set; }
    }

    public class TwoStepKdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.TwoStep;
        public TwoStepCapabilities[] Capabilities { get; set; }
    }

    public class TwoStepCapabilities : Capability
    {
        public MacSaltMethod[] MacSaltMethods { get; set; }
        /// <summary>
        /// The pattern used for FixedInputConstruction.
        /// </summary>
        public string FixedInfoPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        public FixedInfoEncoding[] Encoding { get; set; }
    }

    public class IkeV1Kdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.Ike_v1;
        public HashFunctions[] HashFunctions { get; set; }
    }

    public class IkeV2Kdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.Ike_v2;
        public HashFunctions[] HashFunctions { get; set; }
    }

    public class TlsV10_11Kdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.Tls_v10_v11;
    }

    public class TlsV12Kdf : KdfMethodBase
    {
        public override Kda KdfType => Kda.Tls_v12;
        public HashFunctions[] HashFunctions { get; set; }
    }

    public class KtsMethod
    {
        /// <summary>
        /// The hash algorithms supported for KTS.
        /// </summary>
        public KasHashAlg[] HashAlgs { get; set; }
        /// <summary>
        /// Can the KTS scheme run w/o associated data?
        /// </summary>
        public bool SupportsNullAssociatedData { get; set; }
        /// <summary>
        /// The pattern for construction of associated data.
        /// </summary>
        public string AssociatedDataPattern { get; set; }
        /// <summary>
        /// The encoding type of the fixedInput
        /// </summary>
        public FixedInfoEncoding[] Encoding { get; set; }
    }

    /// <summary>
    /// Options for a MAC used in KeyConfirmation
    /// </summary>
    public class MacMethods
    {
        [JsonProperty(PropertyName = "CMAC")]
        public MacOptionCmac Cmac { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA-1")]
        public MacOptionHmacSha1 HmacSha1 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-224")]
        public MacOptionHmacSha2_d224 HmacSha2_D224 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-256")]
        public MacOptionHmacSha2_d256 HmacSha2_D256 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-384")]
        public MacOptionHmacSha2_d384 HmacSha2_D384 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-512")]
        public MacOptionHmacSha2_d512 HmacSha2_D512 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-512/224")]
        public MacOptionHmacSha2_d512_t224 HmacSha2_D512_T224 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA2-512/256")]
        public MacOptionHmacSha2_d512_t256 HmacSha2_D512_T256 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA3-224")]
        public MacOptionHmacSha3_d224 HmacSha3_D224 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA3-256")]
        public MacOptionHmacSha3_d256 HmacSha3_D256 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA3-384")]
        public MacOptionHmacSha3_d384 HmacSha3_D384 { get; set; }
        [JsonProperty(PropertyName = "HMAC-SHA3-512")]
        public MacOptionHmacSha3_d512 HmacSha3_D512 { get; set; }
        [JsonProperty(PropertyName = "KMAC-128")]
        public MacOptionKmac128 Kmac128 { get; set; }
        [JsonProperty(PropertyName = "KMAC-256")]
        public MacOptionKmac256 Kmac256 { get; set; }

        public IEnumerable<MacOptionsBase> GetRegisteredMacMethods()
        {
            var list = new List<MacOptionsBase>();

            list.AddIfNotNull(Cmac);
            list.AddIfNotNull(HmacSha1);
            list.AddIfNotNull(HmacSha2_D224);
            list.AddIfNotNull(HmacSha2_D256);
            list.AddIfNotNull(HmacSha2_D384);
            list.AddIfNotNull(HmacSha2_D512);
            list.AddIfNotNull(HmacSha2_D512_T224);
            list.AddIfNotNull(HmacSha2_D512_T256);
            list.AddIfNotNull(HmacSha3_D224);
            list.AddIfNotNull(HmacSha3_D256);
            list.AddIfNotNull(HmacSha3_D384);
            list.AddIfNotNull(HmacSha3_D512);
            list.AddIfNotNull(Kmac128);
            list.AddIfNotNull(Kmac256);

            return list;
        }
    }

    /// <summary>
    /// Mac Options shared between MAC types
    /// </summary>
    public abstract class MacOptionsBase
    {
        /// <summary>
        /// The enum MacType
        /// </summary>
        public abstract KeyAgreementMacType MacType { get; }
        /// <summary>
        /// The length of key to be passed into the MAC algorithm.
        /// </summary>
        public int KeyLen { get; set; }
        /// <summary>
        /// The length of the MAC tag to generate.
        /// </summary>
        public int MacLen { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Cmac
    /// </summary>
    public class MacOptionCmac : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.CmacAes;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha-1
    /// </summary>
    public class MacOptionHmacSha1 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha1;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-224
    /// </summary>
    public class MacOptionHmacSha2_d224 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha2D224;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-256
    /// </summary>
    public class MacOptionHmacSha2_d256 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha2D256;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-384
    /// </summary>
    public class MacOptionHmacSha2_d384 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha2D384;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512
    /// </summary>
    public class MacOptionHmacSha2_d512 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha2D512;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512/224
    /// </summary>
    public class MacOptionHmacSha2_d512_t224 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha2D512_T224;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512/256
    /// </summary>
    public class MacOptionHmacSha2_d512_t256 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha2D512_T256;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3-224
    /// </summary>
    public class MacOptionHmacSha3_d224 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha3D224;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3-256
    /// </summary>
    public class MacOptionHmacSha3_d256 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha3D256;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3-384
    /// </summary>
    public class MacOptionHmacSha3_d384 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha3D384;
    }

    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3 512
    /// </summary>
    public class MacOptionHmacSha3_d512 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.HmacSha3D512;
    }

    /// <inheritdoc />
    /// <summary>
    /// KMAC-128
    /// </summary>
    public class MacOptionKmac128 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.Kmac_128;
    }

    /// <inheritdoc />
    /// <summary>
    /// KMAC-256
    /// </summary>
    public class MacOptionKmac256 : MacOptionsBase
    {
        public override KeyAgreementMacType MacType => KeyAgreementMacType.Kmac_256;
    }
}
