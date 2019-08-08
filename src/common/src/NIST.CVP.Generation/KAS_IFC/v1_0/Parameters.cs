using Newtonsoft.Json;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;

namespace NIST.CVP.Generation.KAS_IFC.v1_0
{
    public class Parameters : IParameters 
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }
        
        /// <summary>
        /// KAS Assurances
        /// </summary>
        public string[] Function { get; set; }
        
        /// <summary>
        /// The KAS schemes to test
        /// </summary>
        public Schemes Scheme { get; set; }
    }

    public class Schemes
    {
        [JsonProperty(PropertyName = "KAS1-basic")]
        public Kas1_basic Kas1_basic { get; set; }
        [JsonProperty(PropertyName = "KAS1-Party_V-confirmation")]
        public Kas1_partyV_confirmation Kas1_partyV_confirmation { get; set; }
        [JsonProperty(PropertyName = "KAS2-basic")]
        public Kas2_basic Kas2_basic { get; set; }
        [JsonProperty(PropertyName = "KAS1-bilateral-confirmation")]
        public Kas2_bilateral_confirmation Kas2_bilateral_confirmation { get; set; }
        [JsonProperty(PropertyName = "KAS1-Party_V-confirmation")]
        public Kas2_partyV_confirmation Kas2_partyV_confirmation { get; set; }
        [JsonProperty(PropertyName = "KAS1-Party_U-confirmation")]
        public Kas2_partyU_confirmation Kas2_partyU_confirmation { get; set; }
        [JsonProperty(PropertyName = "KTS-OAEP-basic")]
        public Kts_oaep_basic Kts_oaep_basic { get; set; }
        [JsonProperty(PropertyName = "KTS-OAEP-Party_V-confirmation")]
        public Kts_oaep_partyV_confirmation Kts_oaep_partyV_confirmation { get; set; }
    }

    public abstract class SchemeBase
    {
        /// <summary>
        /// The Key Agreement Role (initiator or responder)
        /// </summary>
        public KeyAgreementRole[] KasRole { get; set; }
        /// <summary>
        /// The key generation methods to test within this scheme.
        /// </summary>
        public KeyGenerationMethods KeyGenerationMethods { get; set; }
        /// <summary>
        /// The KDF types used under this scheme (KAS schemes only)
        /// </summary>
        public KdfMethods KdfMethods { get; set; }
        /// <summary>
        /// The KTS capabilities used (KTS schemes only)
        /// </summary>
        public KtsCapabilities KtsCapabilities { get; set; }
        public MacOptions MacMethods { get; set; }
    }

    public class Kas1_basic : SchemeBase {}
    public class Kas1_partyV_confirmation : SchemeBase {}
    public class Kas2_basic : SchemeBase {}
    public class Kas2_bilateral_confirmation : SchemeBase {}
    public class Kas2_partyV_confirmation : SchemeBase {}
    public class Kas2_partyU_confirmation : SchemeBase {}
    public class Kts_oaep_basic : SchemeBase {}
    public class Kts_oaep_partyV_confirmation : SchemeBase {}

    public class KeyGenerationMethods
    {
        [JsonProperty(PropertyName = "rsakpg1-basic")]
        public RsaKpg1_basic RsaKpg1_basic { get; set; }
        [JsonProperty(PropertyName = "rsakpg1-prime-factor")]
        public RsaKpg1_primeFactor RsaKpg1_primeFactor { get; set; }
        [JsonProperty(PropertyName = "rsakpg1-crt")]
        public RsaKpg1_crt RsaKpg1_crt { get; set; }
        [JsonProperty(PropertyName = "rsakpg2-basic")]
        public RsaKpg2_basic RsaKpg2_basic { get; set; }
        [JsonProperty(PropertyName = "rsakpg2-prime-factor")]
        public RsaKpg2_primeFactor RsaKpg2_primeFactor { get; set; }
        [JsonProperty(PropertyName = "rsakpg2-crt")]
        public RsaKpg2_crt RsaKpg2_crt { get; set; }
    }

    public abstract class KeyGenerationMethodBase
    {
        /// <summary>
        /// The modulus lengths supported
        /// </summary>
        public int[] Modulus { get; set; }
        /// <summary>
        /// The fixed public exponent (only applicable for rsakpg1*)
        /// </summary>
        public BitString FixedPublicExponent { get; set; }
    }
    
    public class RsaKpg1_basic : KeyGenerationMethodBase {}
    public class RsaKpg1_primeFactor : KeyGenerationMethodBase {}
    public class RsaKpg1_crt : KeyGenerationMethodBase {}
    public class RsaKpg2_basic : KeyGenerationMethodBase {}
    public class RsaKpg2_primeFactor : KeyGenerationMethodBase {}
    public class RsaKpg2_crt : KeyGenerationMethodBase {}

    public class KdfMethods
    {
        [JsonProperty(PropertyName = "concatenation")]
        public KdfConcatenation Concatenation { get; set; }
        [JsonProperty(PropertyName = "asn1")]
        public KdfAsn1 Asn1 { get; set; }
    }

    public abstract class KdfMethodBase
    {
        /// <summary>
        /// The Hash or MAC functions utilized for the KDF
        /// </summary>
        public string[] AuxFunctions { get; set; }
        /// <summary>
        /// The salting methods used for the KDF (hashes do not require salts, MACs do)
        /// </summary>
        public MacSaltMethod[] MacSaltMethods { get; set; }
        /// <summary>
        /// The pattern used for OtherInputConstruction.
        /// </summary>
        public string OtherInputPattern { get; set; }
    }
    
    public class KdfConcatenation : KdfMethodBase {}
    public class KdfAsn1 : KdfMethodBase {}

    public class KtsCapabilities
    {
        /// <summary>
        /// The hash algorithms supported for KTS.
        /// </summary>
        public string[] HashAlgs { get; set; }
        /// <summary>
        /// Can the KTS scheme run w/o associated data?
        /// </summary>
        public bool SupportsNullAssociatedData { get; set; }
        /// <summary>
        /// The pattern for construction of associated data.
        /// </summary>
        public string AssociatedDataPattern { get; set; }
    }
    
    /// <summary>
    /// Options for a MAC
    /// </summary>
    public class MacOptions
    {
        [JsonProperty(PropertyName = "AES-CCM")]
        public MacOptionAesCcm AesCcm { get; set; }
        [JsonProperty(PropertyName = "CMAC")]
        public MacOptionCmac Cmac { get; set; }
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
    }

    /// <summary>
    /// Mac Options shared between MAC types
    /// </summary>
    public abstract class MacOptionsBase
    {
        public MathDomain KeyLen { get; set; }
        public int MacLen { get; set; }
        public int NonceLen { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Aes-Ccm
    /// </summary>
    public class MacOptionAesCcm : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Cmac
    /// </summary>
    public class MacOptionCmac : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-224
    /// </summary>
    public class MacOptionHmacSha2_d224 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-256
    /// </summary>
    public class MacOptionHmacSha2_d256 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2-384
    /// </summary>
    public class MacOptionHmacSha2_d384 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512
    /// </summary>
    public class MacOptionHmacSha2_d512 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512/224
    /// </summary>
    public class MacOptionHmacSha2_d512_t224 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha2 512/256
    /// </summary>
    public class MacOptionHmacSha2_d512_t256 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3-224
    /// </summary>
    public class MacOptionHmacSha3_d224 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3-256
    /// </summary>
    public class MacOptionHmacSha3_d256 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3-384
    /// </summary>
    public class MacOptionHmacSha3_d384 : MacOptionsBase { }
    /// <inheritdoc />
    /// <summary>
    /// Hmac Sha3 512
    /// </summary>
    public class MacOptionHmacSha3_d512 : MacOptionsBase { }
    
    
}