using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class KTS_IFC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("iutId")]
		public string IUTID { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KTS_IFC()
		{
			Name = "KTS-IFC";
			Revision = "Sp800-56Br2";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("KTS-OAEP-basic")]
			public SchemeBase KTSOAEPBasic { get; set; }

			[JsonPropertyName("KTS-OAEP-Party_V-confirmation")]
			public SchemeWithMacMethods KTSOAEPPartyVConfirmation { get; set; }
		}

		public class SchemeBase
		{
			[JsonPropertyName("kasRole")]
			public List<string> KasRole { get; set; }

			[JsonPropertyName("keyGenerationMethods")]
			public KeyGenerationMethods KeyGenerationMethods { get; set; }

			[JsonPropertyName("ktsMethod")]
			public KtsMethod KtsMethod { get; set; }

			[JsonPropertyName("l")]
			public int L { get; set; }
		}

		public class SchemeWithMacMethods : SchemeBase
		{
			[JsonPropertyName("macMethods")]
			public MacMethods MacMethods { get; set; }
		}


		public class KeyGenerationMethods
		{
			[JsonPropertyName("rsakpg1-basic")]
			public KeyGenerationMethodModuloFixedPubExp RSAKpg1Basic { get; set; }

			[JsonPropertyName("rsakpg1-prime-factor")]
			public KeyGenerationMethodModuloFixedPubExp RSAKpg1PrimeFactor { get; set; }

			[JsonPropertyName("rsakpg1-crt")]
			public KeyGenerationMethodModuloFixedPubExp RSAKpg1Crt { get; set; }

			[JsonPropertyName("rsakpg2-basic")]
			public KeyGenerationMethodModulo RSAKpg2Basic { get; set; }

			[JsonPropertyName("rsakpg2-prime-factor")]
			public KeyGenerationMethodModulo RSAKpg2PrimeFactor { get; set; }

			[JsonPropertyName("rsakpg2-crt")]
			public KeyGenerationMethodModulo RSAKpg2Crt { get; set; }
		}

		public class KeyGenerationMethodModulo
		{
			[JsonPropertyName("modulo")]
			public List<int> Modulo { get; set; }
		}

		public class KeyGenerationMethodModuloFixedPubExp : KeyGenerationMethodModulo
		{
			[JsonPropertyName("fixedPublicExponent")]
			public string FixedPublicExponent { get; set; }
		}

		public class KtsMethod
		{
			[JsonPropertyName("hashAlgs")]
			public List<string> HashAlgorithms { get; set; }

			[JsonPropertyName("supportsNullAssociatedData")]
			public bool SupportsNullAssociatedData { get; set; }

			[JsonPropertyName("associatedDataPattern")]
			public string AssociatedDataPattern { get; set; }

			[JsonPropertyName("encoding")]
			public List<string> Encoding { get; set; }
		}

		public class MacMethods
		{
			[JsonPropertyName("CMAC")]
			public MacMethod CMAC { get; set; }

			[JsonPropertyName("KMAC-128")]
			public MacMethod KMAC_128 { get; set; }

			[JsonPropertyName("KMAC-256")]
			public MacMethod KMAC_256 { get; set; }

			[JsonPropertyName("HMAC-SHA2-224")]
			public MacMethod HMAC_SHA2_224 { get; set; }

			[JsonPropertyName("HMAC-SHA2-256")]
			public MacMethod HMAC_SHA2_256 { get; set; }

			[JsonPropertyName("HMAC-SHA2-384")]
			public MacMethod HMAC_SHA2_384 { get; set; }

			[JsonPropertyName("HMAC-SHA2-512")]
			public MacMethod HMAC_SHA2_512 { get; set; }

			[JsonPropertyName("HMAC-SHA2-512/224")]
			public MacMethod HMAC_SHA2_512224 { get; set; }

			[JsonPropertyName("HMAC-SHA2-512/256")]
			public MacMethod HMAC_SHA2_512256 { get; set; }

			[JsonPropertyName("HMAC-SHA3-224")]
			public MacMethod HMAC_SHA3_224 { get; set; }

			[JsonPropertyName("HMAC-SHA3-256")]
			public MacMethod HMAC_SHA3_256 { get; set; }

			[JsonPropertyName("HMAC-SHA3-384")]
			public MacMethod HMAC_SHA3_384 { get; set; }

			[JsonPropertyName("HMAC-SHA3-512")]
			public MacMethod HMAC_SHA3_512 { get; set; }
		}

		public class MacMethod
		{
			[JsonPropertyName("keyLen")]
			public int KeyLength { get; set; }

			[JsonPropertyName("macLen")]
			public int MacLength { get; set; }
		}
	}
}
