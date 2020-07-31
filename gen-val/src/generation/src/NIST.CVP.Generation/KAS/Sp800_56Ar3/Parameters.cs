using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper.Enums;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.v1_0;
using NIST.CVP.Generation.KDF.v1_0;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.Sp800_56Ar3
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
        /// The domain parameters that can be used for the registration.
        /// </summary>
        public KasDpGeneration[] DomainParameterGenerationMethods { get; set; }
    }

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
        [JsonProperty(PropertyName = "dhHybridOneFlow")]
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

        public IEnumerable<SchemeBase> GetRegisteredSchemes()
        {
            var list = new List<SchemeBase>();

            list.AddIfNotNull(FfcDhEphem);
            list.AddIfNotNull(FfcMqv1);
            list.AddIfNotNull(FfcDhHybrid1);
            list.AddIfNotNull(FfcMqv2);
            list.AddIfNotNull(FfcDhHybridOneFlow);
            list.AddIfNotNull(FfcDhOneFlow);
            list.AddIfNotNull(FfcDhStatic);
            
            list.AddIfNotNull(EccEphemeralUnified);
            list.AddIfNotNull(EccOnePassMqv);
            list.AddIfNotNull(EccFullUnified);
            list.AddIfNotNull(EccFullMqv);
            list.AddIfNotNull(EccOnePassUnified);
            list.AddIfNotNull(EccOnePassDh);
            list.AddIfNotNull(EccStaticUnified);

            return list;
        }
    }

    public abstract class SchemeBase
    {
        /// <summary>
        /// The enum scheme type
        /// </summary>
        public abstract KasScheme Scheme { get; }
        public abstract KasAlgorithm UnderlyingAlgorithm { get; }
        /// <summary>
        /// The Key Agreement Role (initiator or responder)
        /// </summary>
        public KeyAgreementRole[] KasRole { get; set; }
        /// <summary>
        /// The KDF types used under this scheme (KAS schemes only)
        /// </summary>
        public KdfMethods KdfMethods { get; set; }
        /// <summary>
        /// The KeyConfirmation configuration.
        /// </summary>
        public KeyConfirmationMethod KeyConfirmationMethod { get; set; }
        /// <summary>
        /// The length of the key to derive using a KDF.
        /// </summary>
        /// <remarks>This value should be large enough to accommodate the maximum key length used for a mac algorithm.</remarks>
        public int L { get; set; }
    }

    #region FFC

    /// <inheritdoc />
    /// <summary>
    /// Registration for DiffieHellman ephemeral
    /// </summary>
    public class FfcDhEphem : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhEphem;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for MQV1
    /// </summary>
    public class FfcMqv1 : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcMqv1;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhHybrid1
    /// </summary>
    public class FfcDhHybrid1 : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhHybrid1;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for Mqv2
    /// </summary>
    public class FfcMqv2 : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcMqv2;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhHybridOneFlow
    /// </summary>
    public class FfcDhHybridOneFlow : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhHybridOneFlow;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhOneFlow
    /// </summary>
    public class FfcDhOneFlow : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhOneFlow;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhStatic
    /// </summary>
    public class FfcDhStatic : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhStatic;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
    }
    #endregion FFC

    #region ECC

    /// <inheritdoc />
    /// <summary>
    /// Registration for EphemeralUnified
    /// </summary>
    public class EccEphemeralUnified : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccEphemeralUnified;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassMqv
    /// </summary>
    public class EccOnePassMqv : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccOnePassMqv;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for FullUnified
    /// </summary>
    public class EccFullUnified : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccFullUnified;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for FullMqv
    /// </summary>
    public class EccFullMqv : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccFullMqv;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassUnified
    /// </summary>
    public class EccOnePassUnified : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccOnePassUnified;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassDh
    /// </summary>
    public class EccOnePassDh : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccOnePassDh;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for StaticUnified
    /// </summary>
    public class EccStaticUnified : SchemeBase
    {
        public override KasScheme Scheme => KasScheme.EccStaticUnified;
        public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
    }
    #endregion ECC

    public class KeyConfirmationMethod
    {
        /// <summary>
        /// The MAC used for schemes utilizing KeyConfirmation
        /// </summary>
        public MacMethods MacMethods { get; set; }
        /// <summary>
        /// The supported <see cref="KeyConfirmationDirection"/>s.
        /// </summary>
        public KeyConfirmationDirection[] KeyConfirmationDirections { get; set; }
        /// <summary>
        /// The supported <see cref="KeyConfirmationRole"/>s.
        /// </summary>
        public KeyConfirmationRole[] KeyConfirmationRoles { get; set; }
    }
    
    public class KdfMethods
    {
        public OneStepKdf OneStepKdf { get; set; }
        public TwoStepKdf TwoStepKdf { get; set; }
        public IkeV1Kdf IkeV1Kdf { get; set; }
        public IkeV2Kdf IkeV2Kdf { get; set; }
        public TlsV10_11Kdf TlsV10_11Kdf { get; set; }
        public TlsV12Kdf TlsV12Kdf { get; set; }

        public IEnumerable<KdfMethodBase> GetRegisteredKdfMethods()
        {
            var list = new List<KdfMethodBase>();

            list.AddIfNotNull(OneStepKdf);
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
        public abstract KasKdf KdfType { get; }
    }

    public class OneStepKdf : KdfMethodBase
    {
        public override KasKdf KdfType => KasKdf.OneStep;
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

    public class TwoStepKdf : KdfMethodBase
    {
        public override KasKdf KdfType => KasKdf.TwoStep;
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
        public override KasKdf KdfType => KasKdf.Ike_v1;
        public HashFunctions[] HashFunctions { get; set; }
    }

    public class IkeV2Kdf : KdfMethodBase
    {
        public override KasKdf KdfType => KasKdf.Ike_v2;
        public HashFunctions[] HashFunctions { get; set; }
    }

    public class TlsV10_11Kdf : KdfMethodBase
    {
        public override KasKdf KdfType => KasKdf.Tls_v10_v11;
    }

    public class TlsV12Kdf : KdfMethodBase
    {
        public override KasKdf KdfType => KasKdf.Tls_v12;
        public HashFunctions[] HashFunctions { get; set; }
    }

    /// <summary>
    /// Options for a MAC used in KeyConfirmation
    /// </summary>
    public class MacMethods
    {
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
        [JsonProperty(PropertyName = "KMAC-128")]
        public MacOptionKmac128 Kmac128 { get; set; }
        [JsonProperty(PropertyName = "KMAC-256")]
        public MacOptionKmac256 Kmac256 { get; set; }

        public IEnumerable<MacOptionsBase> GetRegisteredMacMethods()
        {
            var list = new List<MacOptionsBase>();

            list.AddIfNotNull(Cmac);
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