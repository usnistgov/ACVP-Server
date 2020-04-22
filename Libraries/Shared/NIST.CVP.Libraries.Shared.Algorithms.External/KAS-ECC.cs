using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_ECC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_ECC()
		{
			Name = "KAS-ECC";
			Revision = "1.0";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("fullUnified")]
			public Scheme FullUnified { get; set; }

			[JsonPropertyName("fullMqv")]
			public Scheme FullMQV { get; set; }

			[JsonPropertyName("ephemeralUnified")]
			public SchemeEphemeralUnified EphemeralUnified { get; set; }

			[JsonPropertyName("onePassUnified")]
			public Scheme OnePassUnified { get; set; }

			[JsonPropertyName("onePassMqv")]
			public Scheme OnePassMQV { get; set; }

			[JsonPropertyName("onePassDh")]
			public Scheme OnePassDH { get; set; }

			[JsonPropertyName("staticUnified")]
			public SchemeStaticUnified StaticUnified { get; set; }
		}

		public class Scheme
		{
			[JsonPropertyName("kasRole")]
			public List<string> Role { get; set; }

			[JsonPropertyName("kdfNoKc")]
			public KdfNoKc KdfNoKc { get; set; }

			[JsonPropertyName("kdfKc")]
			public KdfKc KdfKc { get; set; }
		}

		public class SchemeEphemeralUnified
		{
			[JsonPropertyName("kasRole")]
			public List<string> Role { get; set; }

			[JsonPropertyName("kdfNoKc")]
			public KdfNoKcStaticUnified KdfNoKc { get; set; }
		}

		public class SchemeStaticUnified
		{
			[JsonPropertyName("kasRole")]
			public List<string> Role { get; set; }

			[JsonPropertyName("kdfNoKc")]
			public KdfNoKcStaticUnified KdfNoKc { get; set; }

			[JsonPropertyName("kdfKc")]
			public KdfKcStaticUnified KdfKc { get; set; }
		}

		public class KdfNoKc
		{
			[JsonPropertyName("kdfOption")]
			public KdfOptions KdfOption { get; set; }

			[JsonPropertyName("parameterSet")]
			public ParameterSets ParameterSets { get; set; }
		}

		public class KdfNoKcStaticUnified : KdfNoKc
		{
			[JsonPropertyName("dkmNonceTypes")]
			public List<string> DkmNonceTypes { get; set; }
		}


		public class KdfKc
		{
			[JsonPropertyName("kcOption")]
			public KcOptions KcOption { get; set; }

			[JsonPropertyName("kdfOption")]
			public KdfOptions KdfOption { get; set; }

			[JsonPropertyName("parameterSet")]
			public ParameterSets ParameterSets { get; set; }
		}

		public class KdfKcStaticUnified : KdfKc
		{
			[JsonPropertyName("dkmNonceTypes")]
			public List<string> DkmNonceTypes { get; set; }
		}

		public class ParameterSets
		{
			[JsonPropertyName("eb")]
			public ParameterSetEB EB { get; set; }

			[JsonPropertyName("ec")]
			public ParameterSetEC EC { get; set; }

			[JsonPropertyName("ed")]
			public ParameterSetED ED { get; set; }

			[JsonPropertyName("ee")]
			public ParameterSetEE EE { get; set; }
		}

		public abstract class ParameterSetBase
		{
			[JsonPropertyName("curve")]
			public string Curve { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlg { get; set; }
		}

		public class ParameterSetEB : ParameterSetBase
		{
			[JsonPropertyName("macOption")]
			public MacOptionsEB MacOption { get; set; }
		}

		public class ParameterSetEC : ParameterSetBase
		{
			[JsonPropertyName("macOption")]
			public MacOptionsEC MacOption { get; set; }
		}

		public class ParameterSetED : ParameterSetBase
		{
			[JsonPropertyName("macOption")]
			public MacOptionsED MacOption { get; set; }
		}

		public class ParameterSetEE : ParameterSetBase
		{
			[JsonPropertyName("macOption")]
			public MacOptionsEE MacOption { get; set; }
		}

		public class KdfOptions
		{
			[JsonPropertyName("concatenation")]
			public string Concatenation { get; set; }

			[JsonPropertyName("asn1")]
			public string Asn1 { get; set; }
		}


		public abstract class MacOptionsBase
		{
			[JsonPropertyName("AES-CCM")]
			public MacOptionWithNonceLen AesCcm { get; set; }

			[JsonPropertyName("CMAC")]
			public MacOption Cmac { get; set; }

			[JsonPropertyName("HMAC-SHA2-512")]
			public MacOption HmacSha2_D512 { get; set; }
		}

		public class MacOptionsEE : MacOptionsBase { }

		public class MacOptionsED : MacOptionsBase
		{
			[JsonPropertyName("HMAC-SHA2-384")]
			public MacOption HmacSha2_D384 { get; set; }
		}

		public class MacOptionsEC : MacOptionsBase
		{
			[JsonPropertyName("HMAC-SHA2-256")]
			public MacOption HmacSha2_D256 { get; set; }

			[JsonPropertyName("HMAC-SHA2-384")]
			public MacOption HmacSha2_D384 { get; set; }
		}


		public class MacOptionsEB : MacOptionsBase
		{
			[JsonPropertyName("HMAC-SHA2-224")]
			public MacOption HmacSha2_D224 { get; set; }

			[JsonPropertyName("HMAC-SHA2-256")]
			public MacOption HmacSha2_D256 { get; set; }

			[JsonPropertyName("HMAC-SHA2-384")]
			public MacOption HmacSha2_D384 { get; set; }
		}


		public class MacOption
		{
			[JsonPropertyName("keyLen")]
			public Domain KeyLen { get; set; }

			[JsonPropertyName("macLen")]
			public int MacLen { get; set; }
		}

		public class MacOptionWithNonceLen : MacOption
		{
			[JsonPropertyName("nonceLen")]
			public int NonceLen { get; set; }
		}


		public class KcOptions
		{
			[JsonPropertyName("kcRole")]
			public List<string> KcRole { get; set; }

			[JsonPropertyName("kcType")]
			public List<string> KcType { get; set; }

			[JsonPropertyName("nonceType")]
			public List<string> NonceType { get; set; }
		}
	}
}
