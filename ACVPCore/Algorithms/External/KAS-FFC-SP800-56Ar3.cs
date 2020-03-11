using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class KAS_FFC_SP800_56Ar3 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("domainParameterGenerationMethods")]
		public List<string> DomainParameterGenerationMethods { get; set; }

		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("iutId")]
		public string IUTID { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_FFC_SP800_56Ar3()
		{
			Name = "KAS-FFC";
			Revision = "Sp800-56Ar3";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("dhHybrid1")]
			public Scheme DhHybrid1 { get; set; }

			[JsonPropertyName("mqv2")]
			public Scheme MQV2 { get; set; }

			[JsonPropertyName("dhEphem")]
			public SchemeNoKc DhEphemeral { get; set; }

			[JsonPropertyName("dhHybridOneFlow")]
			public Scheme DhHybridOneFlow { get; set; }

			[JsonPropertyName("mqv1")]
			public Scheme MQV1 { get; set; }

			[JsonPropertyName("dhOneFlow")]
			public Scheme DhOneFlow { get; set; }

			[JsonPropertyName("dhStatic")]
			public Scheme DhStatic { get; set; }
		}

		public class Scheme
		{
			[JsonPropertyName("kasRole")]
			public List<string> KasRole { get; set; }

			[JsonPropertyName("kdfMethods")]
			public KdfMethods KdfMethods { get; set; }

			[JsonPropertyName("keyConfirmationMethod")]
			public KeyConfirmationMethod KeyConfirmationMethod { get; set; }

			[JsonPropertyName("l")]
			public int L { get; set; }
		}

		public class SchemeNoKc
		{
			[JsonPropertyName("kasRole")]
			public List<string> KasRole { get; set; }

			[JsonPropertyName("kdfMethods")]
			public KdfMethods KdfMethods { get; set; }

			[JsonPropertyName("l")]
			public int L { get; set; }
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

		public class KeyConfirmationMethod
		{
			[JsonPropertyName("keyConfirmationDirections")]
			public List<string> KeyConfirmationDirections { get; set; }

			[JsonPropertyName("keyConfirmationRoles")]
			public List<string> KeyConfirmationRoles { get; set; }

			[JsonPropertyName("macMethods")]
			public MacMethods MacMethods { get; set; }
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
