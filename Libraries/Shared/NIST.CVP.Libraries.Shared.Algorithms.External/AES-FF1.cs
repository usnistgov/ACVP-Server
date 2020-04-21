using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class AES_FF1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("direction")]
		public List<string> Direction { get; set; }

		[JsonPropertyName("keyLen")]
		public List<int> KeyLength{ get; set; }

		[JsonPropertyName("tweakLen")]
		public Domain TweakLength { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public AES_FF1()
		{
			Name = "ACVP-AES-FF1";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("alphabet")]
			public string Alphabet { get; set; }

			[JsonPropertyName("radix")]
			public int Radix { get; set; }

			[JsonPropertyName("minLen")]
			public int MinLength { get; set; }

			[JsonPropertyName("maxLen")]
			public int MaxLength { get; set; }
		}
	}
}
