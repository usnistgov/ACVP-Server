using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class KAS_FFC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_FFC()
		{
			Name = "KAS-FFC";
			Revision = "1.0";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("dhEphem")]
			public SchemeNoKC DhEphem { get; set; }

			[JsonPropertyName("mqv1")]
			public SchemeKC Mqv1 { get; set; }

			[JsonPropertyName("dhHybrid1")]
			public SchemeKC DhHybrid1 { get; set; }

			[JsonPropertyName("mqv2")]
			public SchemeKC Mqv2 { get; set; }

			[JsonPropertyName("hybridOneFlow")]
			public SchemeKC DhHybridOneFlow { get; set; }

			[JsonPropertyName("dhOneFlow")]
			public SchemeKC DhOneFlow { get; set; }

			[JsonPropertyName("dhStatic")]
			public SchemeKC DhStatic { get; set; }
		}

		public abstract class SchemeBase
		{
			[JsonPropertyName("kasRole")]
			public List<string> Role { get; set; }

			[JsonPropertyName("kdfNoKc")]
			public KdfNoKc KdfNoKc { get; set; }
		}

		public class SchemeNoKC : SchemeBase { }

		public class SchemeKC : SchemeBase
		{
			[JsonPropertyName("kdfKc")]
			public KdfKc KdfKc { get; set; }
		}

		public abstract class KdfBase
		{
			[JsonPropertyName("kdfOption")]
			public KdfOptions KdfOption { get; set; }

			[JsonPropertyName("parameterSet")]
			public ParameterSets ParameterSets { get; set; }
		}


		public class KdfNoKc : KdfBase { }

		public class KdfKc : KdfBase
		{
			[JsonPropertyName("kcOption")]
			public KcOptions KcOption { get; set; }
		}

		public class ParameterSets
		{
			[JsonPropertyName("fb")]
			public ParameterSetFB FB { get; set; }

			[JsonPropertyName("fc")]
			public ParameterSetFC FC { get; set; }
		}

		public abstract class ParameterSetBase
		{
			[JsonPropertyName("hashAlg")]
			public List<string> HashAlg { get; set; }
		}

		public class ParameterSetFB : ParameterSetBase
		{
			[JsonPropertyName("macOption")]
			public MacOptionsFB MacOption { get; set; }
		}

		public class ParameterSetFC : ParameterSetBase
		{
			[JsonPropertyName("macOption")]
			public MacOptionsFC MacOption { get; set; }
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

			[JsonPropertyName("HMAC-SHA2-256")]
			public MacOption HmacSha2_D256 { get; set; }

			[JsonPropertyName("HMAC-SHA2-384")]
			public MacOption HmacSha2_D384 { get; set; }

			[JsonPropertyName("HMAC-SHA2-512")]
			public MacOption HmacSha2_D512 { get; set; }
		}

		public class MacOptionsFB : MacOptionsBase
		{
			[JsonPropertyName("HMAC-SHA2-224")]
			public MacOption HmacSha2_D224 { get; set; }
		}

		public class MacOptionsFC : MacOptionsBase { }

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
