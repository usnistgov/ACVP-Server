using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class KAS_ECC_SSC_SP800_56Ar3 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("domainParameterGenerationMethods")]
		public List<string> DomainParameterGenerationMethods { get; set; }

		[JsonPropertyName("hashFunctionZ")]
		public string HashFunctionZ { get; set; }
		
		[JsonPropertyName("scheme")] public SchemeCollection Schemes { get; set; }

		public KAS_ECC_SSC_SP800_56Ar3()
		{
			Name = "KAS-ECC-SSC";
			Revision = "Sp800-56Ar3";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("fullUnified")] public Scheme FullUnified { get; set; }

			[JsonPropertyName("fullMqv")] public Scheme FullMQV { get; set; }

			[JsonPropertyName("ephemeralUnified")] public Scheme EphemeralUnified { get; set; }

			[JsonPropertyName("onePassUnified")] public Scheme OnePassUnified { get; set; }

			[JsonPropertyName("onePassMqv")] public Scheme OnePassMQV { get; set; }

			[JsonPropertyName("onePassDh")] public Scheme OnePassDH { get; set; }

			[JsonPropertyName("staticUnified")] public Scheme StaticUnified { get; set; }
		}

		public class Scheme
		{
			[JsonPropertyName("kasRole")] public List<string> KasRole { get; set; }
		}
	}
}
