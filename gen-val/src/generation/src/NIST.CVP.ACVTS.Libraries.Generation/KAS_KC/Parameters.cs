using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.KAS_KC
{
    public class Parameters : IParameters
    {
        public int VectorSetId { get; set; }
        public string Algorithm { get; set; }
        public string Mode { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        public KeyAgreementRole[] KasRole { get; set; }
        public KeyConfirmationMethod KeyConfirmationMethod { get; set; }
    }

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
    /// Hmac Sha2-224
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
