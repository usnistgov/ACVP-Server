using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class KAS_FFC_Component : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("function")]
		public List<string> Functions { get; set; }

		[JsonPropertyName("scheme")]
		public SchemeCollection Schemes { get; set; }

		public KAS_FFC_Component()
		{
			Name = "KAS-FFC";
			Mode = "Component";
			Revision = "1.0";
		}

		public class SchemeCollection
		{
			[JsonPropertyName("dhEphem")]
			public Scheme DhEphem { get; set; }

			[JsonPropertyName("mqv1")]
			public Scheme Mqv1 { get; set; }

			[JsonPropertyName("dhHybrid1")]
			public Scheme DhHybrid1 { get; set; }

			[JsonPropertyName("mqv2")]
			public Scheme Mqv2 { get; set; }

			[JsonPropertyName("hybridOneFlow")]
			public Scheme DhHybridOneFlow { get; set; }

			[JsonPropertyName("dhOneFlow")]
			public Scheme DhOneFlow { get; set; }

			[JsonPropertyName("dhStatic")]
			public Scheme DhStatic { get; set; }
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
			[JsonPropertyName("fb")]
			public ParameterSet FB { get; set; }

			[JsonPropertyName("fc")]
			public ParameterSet FC { get; set; }
		}

		public class ParameterSet
		{
			[JsonPropertyName("hashAlg")]
			public List<string> HashAlg { get; set; }
		}
	}
}
