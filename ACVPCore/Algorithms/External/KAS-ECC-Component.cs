using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class KAS_ECC_Component : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_ECC_Component()
		{
			Name = "KAS-ECC";
			Mode = "Component";
			Revision = "1.0";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("ephemeralUnified")]
			public Scheme EphemeralUnified { get; set; }

			[JsonPropertyName("fullUnified")]
			public Scheme FullUnified { get; set; }

			[JsonPropertyName("fullMqv")]
			public Scheme FullMQV { get; set; }

			[JsonPropertyName("onePassUnified")]
			public Scheme OnePassUnified { get; set; }

			[JsonPropertyName("onePassMqv")]
			public Scheme OnePassMQV { get; set; }

			[JsonPropertyName("onePassDh")]
			public Scheme OnePassDH { get; set; }

			[JsonPropertyName("staticUnified")]
			public Scheme StaticUnified { get; set; }
		}

		public class Scheme
		{
			[JsonPropertyName("kasRole")]
			public List<string> Role { get; set; }

			[JsonPropertyName("noKdfNoKc")]
			public NoKdfNoKc NoKdfNoKc { get; set; }
		}

		public class NoKdfNoKc
		{
			[JsonPropertyName("parameterSet")]
			public ParameterSets ParameterSets { get; set; }
		}

		public class ParameterSets
		{
			[JsonPropertyName("eb")]
			public ParameterSet EB { get; set; }

			[JsonPropertyName("ec")]
			public ParameterSet EC { get; set; }

			[JsonPropertyName("ed")]
			public ParameterSet ED { get; set; }

			[JsonPropertyName("ee")]
			public ParameterSet EE { get; set; }
		}

		public class ParameterSet
		{
			[JsonPropertyName("curve")]
			public string Curve { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlg { get; set; }
		}
	}
}
