using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Common;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Sp800_56Ar3.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.KAS.Sp800_56Ar3.Enums;

namespace NIST.CVP.Generation.KAS_SSC.Sp800_56Ar3
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
		/// The schemes to test against.
		/// </summary>
		public Schemes Scheme { get; set; }
		
		/// <summary>
		/// The methods of domain parameter generation to test
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
		/// The <see cref="KasScheme"/> registered.
		/// </summary>
		public abstract KasScheme Scheme { get; }
		/// <summary>
		/// Is the <see cref="KasAlgorithm"/> FFC or ECC?
		/// (Is there a better name for this property?)
		/// </summary>
		public abstract KasAlgorithm UnderlyingAlgorithm { get; }
		/// <summary>
		/// The <see cref="AlgoMode"/> the scheme belongs to.
		/// </summary>
		public abstract AlgoMode AlgoMode { get; }
		/// <summary>
		/// The registered <see cref="KeyAgreementRole"/>s roles. (initiator or responder)
		/// </summary>
		public KeyAgreementRole[] KasRole { get; set; }
	}

	public abstract class SchemeFfcBase : SchemeBase
	{
		public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ffc;
		public override AlgoMode AlgoMode => AlgoMode.KAS_FFC_SSC_Sp800_56Ar3;
	}
	
	public abstract class SchemeEccBase : SchemeBase
	{
		public override KasAlgorithm UnderlyingAlgorithm => KasAlgorithm.Ecc;
		public override AlgoMode AlgoMode => AlgoMode.KAS_ECC_SSC_Sp800_56Ar3;
	}
	
	#region FFC
	/// <inheritdoc />
    /// <summary>
    /// Registration for DiffieHellman ephemeral
    /// </summary>
    public class FfcDhEphem : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhEphem;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for MQV1
    /// </summary>
    public class FfcMqv1 : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcMqv1;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhHybrid1
    /// </summary>
    public class FfcDhHybrid1 : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhHybrid1;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for Mqv2
    /// </summary>
    public class FfcMqv2 : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcMqv2;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhHybridOneFlow
    /// </summary>
    public class FfcDhHybridOneFlow : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhHybridOneFlow;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhOneFlow
    /// </summary>
    public class FfcDhOneFlow : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhOneFlow;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for DhStatic
    /// </summary>
    public class FfcDhStatic : SchemeFfcBase
    {
        public override KasScheme Scheme => KasScheme.FfcDhStatic;
    }
    #endregion FFC

    #region ECC

    /// <inheritdoc />
    /// <summary>
    /// Registration for EphemeralUnified
    /// </summary>
    public class EccEphemeralUnified : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccEphemeralUnified;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassMqv
    /// </summary>
    public class EccOnePassMqv : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccOnePassMqv;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for FullUnified
    /// </summary>
    public class EccFullUnified : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccFullUnified;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for FullMqv
    /// </summary>
    public class EccFullMqv : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccFullMqv;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassUnified
    /// </summary>
    public class EccOnePassUnified : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccOnePassUnified;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for OnePassDh
    /// </summary>
    public class EccOnePassDh : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccOnePassDh;
    }

    /// <inheritdoc />
    /// <summary>
    /// Registration for StaticUnified
    /// </summary>
    public class EccStaticUnified : SchemeEccBase
    {
        public override KasScheme Scheme => KasScheme.EccStaticUnified;
    }
    #endregion ECC
}