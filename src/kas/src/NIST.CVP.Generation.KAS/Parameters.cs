using Newtonsoft.Json;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.KAS
{
    public class Parameters : IParameters
    {
        /// <inheritdoc />
        public string Algorithm { get; set; }
        /// <inheritdoc />
        public string Mode { get; set; }
        /// <inheritdoc />
        public bool IsSample { get; set; }
        /// <summary>
        /// KAS Assurances
        /// </summary>
        public string[] Function { get; set; }
        /// <summary>
        /// The KAS schemes to test
        /// </summary>
        public Schemes Scheme { get; set; }
    }

    /// <summary>
    /// Wraps the possible KAS schemes
    /// </summary>
    public class Schemes
    {
        #region FFC
        /// <summary>
        /// FFC Diffie Hellman Ephemeral
        /// </summary>
        [JsonProperty(PropertyName = "dhEphem")]
        public FfcDhEphem FfcDhEphem { get; set; }
        /// <summary>
        /// FFC MQV1
        /// </summary>
        [JsonProperty(PropertyName = "mqv1")]
        public FfcMqv1 FfcMqv1 { get; set; }
        /// <summary>
        /// FFC DhHybrid1
        /// </summary>
        [JsonProperty(PropertyName = "dhHybrid1")]
        public FfcDhHybrid1 FfcDhHybrid1 { get; set; }
        /// <summary>
        /// FFC MQV2
        /// </summary>
        [JsonProperty(PropertyName = "mqv2")]
        public FfcMqv2 FfcMqv2 { get; set; }
        /// <summary>
        /// FFC DhHybridOneFlow
        /// </summary>
        [JsonProperty(PropertyName = "hybridOneFlow")]
        public FfcDhHybridOneFlow FfcDhHybridOneFlow { get; set; }
        /// <summary>
        /// FFC DhOneFLow
        /// </summary>
        [JsonProperty(PropertyName = "dhOneFlow")]
        public FfcDhOneFlow FfcDhOneFlow { get; set; }
        /// <summary>
        /// FFC DhStatic
        /// </summary>
        [JsonProperty(PropertyName = "dhStatic")]
        public FfcDhStatic FfcDhStatic { get; set; }
        #endregion FFC

        #region ECC
        /// <summary>
        /// ECC Ephemeral Unified
        /// </summary>
        [JsonProperty(PropertyName = "ephemeralUnified")]
        public EccEphemeralUnified EccEphemeralUnified { get; set; }
        /// <summary>
        /// ECC OnePassMqv
        /// </summary>
        [JsonProperty(PropertyName = "onePassMqv")]
        public EccOnePassMqv EccOnePassMqv { get; set; }
        /// <summary>
        /// ECC FullUnified
        /// </summary>
        [JsonProperty(PropertyName = "fullUnified")]
        public EccFullUnified EccFullUnified { get; set; }
        /// <summary>
        /// ECC FullMqv
        /// </summary>
        [JsonProperty(PropertyName = "fullMqv")]
        public EccFullMqv EccFullMqv { get; set; }
        /// <summary>
        /// ECC OnePassUnified
        /// </summary>
        [JsonProperty(PropertyName = "onePassUnified")]
        public EccOnePassUnified EccOnePassUnified { get; set; }
        /// <summary>
        /// ECC OnePassDh
        /// </summary>
        [JsonProperty(PropertyName = "onePassDh")]
        public EccOnePassDh EccOnePassDh { get; set; }
        /// <summary>
        /// ECC StaticUnified
        /// </summary>
        [JsonProperty(PropertyName = "staticUnified")]
        public EccStaticUnified EccStaticUnified { get; set; }
        #endregion ECC
    }

    /// <summary>
    /// Describes information needed for a scheme
    /// </summary>
    public abstract class SchemeBase
    {
        /// <summary>
        /// The Key Agreement ROles
        /// </summary>
        public string[] Role { get; set; }
        /// <summary>
        /// Registration options for NoKdfNoKc (Component only test)
        /// </summary>
        public NoKdfNoKc NoKdfNoKc { get; set; }
        /// <summary>
        /// Registration options for KdfNoKc
        /// </summary>
        public KdfNoKc KdfNoKc { get; set; }
        /// <summary>
        /// Registration options for KdfKc
        /// </summary>
        public KdfKc KdfKc { get; set; }
    }

    #region FFC
    /// <inheritdoc />
    /// <summary>
    /// Registration for DiffieHellman ephemeral
    /// </summary>
    public class FfcDhEphem : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for MQV1
    /// </summary>
    public class FfcMqv1 : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhHybrid1
    /// </summary>
    public class FfcDhHybrid1 : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for Mqv2
    /// </summary>
    public class FfcMqv2 : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhHybridOneFlow
    /// </summary>
    public class FfcDhHybridOneFlow : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhOneFlow
    /// </summary>
    public class FfcDhOneFlow : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhStatic
    /// </summary>
    public class FfcDhStatic : SchemeBase { }
    #endregion FFC

    #region ECC
    /// <inheritdoc />
    /// <summary>
    /// Registration for EphemeralUnified
    /// </summary>
    public class EccEphemeralUnified : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassMqv
    /// </summary>
    public class EccOnePassMqv : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for FullUnified
    /// </summary>
    public class EccFullUnified : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for FullMqv
    /// </summary>
    public class EccFullMqv : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassUnified
    /// </summary>
    public class EccOnePassUnified : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassDh
    /// </summary>
    public class EccOnePassDh : SchemeBase { }

    /// <inheritdoc />
    /// <summary>
    /// Registration for StaticUnified
    /// </summary>
    public class EccStaticUnified : SchemeBase { }
    #endregion ECC

    /// <summary>
    /// Registration requirements for NoKdfNoKc
    /// </summary>
    public class NoKdfNoKc
    {
        /// <summary>
        /// The parameter sets used by the scheme
        /// </summary>
        public ParameterSets ParameterSet { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration requirements for KdfNoKc
    /// </summary>
    public class KdfNoKc : NoKdfNoKc
    {
        /// <summary>
        /// KDF specific options
        /// </summary>
        public KdfOptions KdfOption { get; set; }
        /// <summary>
        /// Nonce types used in DKM construction (applies to static schemes only)
        /// </summary>
        public string[] DkmNonceTypes { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration requirements for Kc
    /// </summary>
    public class KdfKc : KdfNoKc
    {
        /// <summary>
        /// KC specific options
        /// </summary>
        public KcOptions KcOption { get; set; }
    }

    /// <summary>
    /// The possible parameter sets for the scheme
    /// </summary>
    public class ParameterSets
    {
        #region FFC
        /// <summary>
        /// The FFC FB parameter set
        /// </summary>
        public Fb Fb { get; set; }
        /// <summary>
        /// The FFC FC parameter set
        /// </summary>
        public Fc Fc { get; set; }
        #endregion FFC
        #region ECC
        /// <summary>
        /// The ECC Eb parameter set
        /// </summary>
        public Eb Eb { get; set; }
        /// <summary>
        /// The ECC Ec parameter set
        /// </summary>
        public Ec Ec { get; set; }
        /// <summary>
        /// The ECC Ed parameter set
        /// </summary>
        public Ed Ed { get; set; }
        /// <summary>
        /// The ECC Ee parameter set
        /// </summary>
        public Ee Ee { get; set; }
        #endregion ECC
    }

    /// <summary>
    /// The options for a parameter set
    /// </summary>
    public abstract class ParameterSetBase
    {
        /// <summary>
        /// Array of SHA modes used for DSA
        /// </summary>
        public string[] HashAlg { get; set; }
        /// <summary>
        /// The MAC options for NoKc and Kc
        /// </summary>
        public MacOptions MacOption { get; set; }
        /// <summary>
        /// The curve to use in ECC schemes
        /// </summary>
        public string CurveName { get; set; }
    }

    /// <inheritdoc />
    /// <summary>Fb</summary>
    public class Fb : ParameterSetBase { }

    /// <inheritdoc />
    /// <summary>Fc</summary>
    public class Fc : ParameterSetBase { }

    /// <inheritdoc />
    /// <summary>Eb</summary>
    public class Eb : ParameterSetBase { }

    /// <inheritdoc />
    /// <summary>Ec</summary>
    public class Ec : ParameterSetBase { }

    /// <inheritdoc />
    /// <summary>Ed</summary>
    public class Ed : ParameterSetBase { }

    /// <inheritdoc />
    /// <summary>Ee</summary>
    public class Ee : ParameterSetBase { }

    /// <summary>
    /// The options for a Kdf function
    /// </summary>
    public class KdfOptions
    {
        public string Concatenation { get; set; }
        public string Asn1 { get; set; }
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
    }

    /// <summary>
    /// Mac Options shared between MAC types
    /// </summary>
    public abstract class MacOptionsBase
    {
        public int[] KeyLen { get; set; }
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

    /// <summary>
    /// The key confirmation options
    /// </summary>
    public class KcOptions
    {
        /// <summary>
        /// The key confirmation roles supported
        /// </summary>
        public string[] KcRole { get; set; }
        /// <summary>
        /// The Key confirmation types supported
        /// </summary>
        public string[] KcType { get; set; }
        /// <summary>
        /// The nonce types supported
        /// </summary>
        public string[] NonceType { get; set; }
    }
}