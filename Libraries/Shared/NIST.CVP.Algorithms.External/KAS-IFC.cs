using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class KAS_IFC : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("iutId")]
		public string IUTID { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_IFC()
		{
			Name = "KAS-IFC";
			Revision = "Sp800-56Br2";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("KAS1-basic")]
			public SchemeBase KAS1Basic { get; set; }

			[JsonPropertyName("KAS2-basic")]
			public SchemeBase KAS2Basic { get; set; }

			[JsonPropertyName("KAS1-Party_V-confirmation")]
			public SchemeWithMacMethods KAS1PartyVConfirmation { get; set; }

			[JsonPropertyName("KAS2-bilateral-confirmation")]
			public SchemeWithMacMethods KAS2BilateralConfirmation { get; set; }

			[JsonPropertyName("KAS2-Party_U-confirmation")]
			public SchemeWithMacMethods KAS2PartyUConfirmation { get; set; }

			[JsonPropertyName("KAS2-Party_V-confirmation")]
			public SchemeWithMacMethods KAS2PartyVConfirmation { get; set; }
		}

		public class SchemeBase
		{
			[JsonPropertyName("kasRole")]
			public List<string> KasRole { get; set; }

			[JsonPropertyName("keyGenerationMethods")]
			public KeyGenerationMethods KeyGenerationMethods { get; set; }

			[JsonPropertyName("kdfMethods")]
			public KdfMethods KdfMethods { get; set; }

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
			[JsonPropertyName("fixedPubExp")]
			public string FixedPublicExponent { get; set; }
		}

		public class KdfMethods
		{
			[JsonPropertyName("oneStepKdf")]
			public OneStepKdf OneStepKdf { get; set; }

			[JsonPropertyName("twoStepKdf")]
			public TwoStepKdf TwoStepKdf { get; set; }
		}

		public class OneStepKdf
		{
			[JsonPropertyName("auxFunctions")]
			public List<AuxFunction> AuxFunctions { get; set; }

			[JsonPropertyName("fixedInfoPattern")]
			public string FixedInfoPattern { get; set; }

			[JsonPropertyName("encoding")]
			public List<string> Encoding { get; set; }
		}

		public class AuxFunction
		{
			[JsonPropertyName("auxFunctionName")]
			public string AuxFunctionName { get; set; }

			[JsonPropertyName("macSaltMethods")]
			public List<string> MacSaltMethods { get; set; }
		}

		public class TwoStepKdf
		{
			[JsonPropertyName("capabilities")]
			public List<Capability> Capabilities { get; set; }
		}

		public class Capability
		{
			[JsonPropertyName("macSaltMethods")]
			public List<string> MacSaltMethods { get; set; }

			[JsonPropertyName("fixedInfoPattern")]
			public string FixedInfoPattern { get; set; }

			[JsonPropertyName("encoding")]
			public List<string> Encoding { get; set; }

			[JsonPropertyName("kdfMode")]
			public string KdfMode { get; set; }

			[JsonPropertyName("macMode")]
			public List<string> MacMode { get; set; }

			[JsonPropertyName("fixedDataOrder")]
			public List<string> FixedDataOrder { get; set; }

			[JsonPropertyName("counterLength")]
			public List<int> CounterLength { get; set; }

			[JsonPropertyName("supportsEmptyIv")]
			public bool SupportsEmptyIV { get; set; }

			[JsonPropertyName("requiresEmptyIv")]
			public bool RequiresEmptyIV { get; set; }

			[JsonPropertyName("supportedLengths")]
			public Domain SupportedLengths { get; set; }
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
