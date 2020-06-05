using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_FFC_SSC_SP800_56Ar3 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("domainParameterGenerationMethods")]
		public List<string> DomainParameterGenerationMethods { get; set; }

		[JsonPropertyName("hashFunctionZ")]
		public string HashFunctionZ { get; set; }
		
		[JsonPropertyName("scheme")] public SchemeCollection Schemes { get; set; }

		public KAS_FFC_SSC_SP800_56Ar3()
		{
			Name = "KAS-FFC-SSC";
			Revision = "Sp800-56Ar3";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("dhHybrid1")] public Scheme DhHybrid1 { get; set; }

			[JsonPropertyName("mqv2")] public Scheme MQV2 { get; set; }

			[JsonPropertyName("dhEphem")] public Scheme DhEphemeral { get; set; }

			[JsonPropertyName("dhHybridOneFlow")] public Scheme DhHybridOneFlow { get; set; }

			[JsonPropertyName("mqv1")] public Scheme MQV1 { get; set; }

			[JsonPropertyName("dhOneFlow")] public Scheme DhOneFlow { get; set; }

			[JsonPropertyName("dhStatic")] public Scheme DhStatic { get; set; }
		}

		public class Scheme
		{
			[JsonPropertyName("kasRole")] public List<string> KasRole { get; set; }
		}
	}
}
