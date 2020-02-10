using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class RSASigGen186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public RSASigGen186_4()
		{
			Name = "RSA";
			Mode = "sigGen";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("sigType")]
			public string SignatureType { get; set; }

			[JsonPropertyName("properties")]
			public List<Property> Properties { get; set; }
		}

		public class Property
		{
			[JsonPropertyName("modulo")]
			public int Modulo { get; set; }

			[JsonPropertyName("hashPair")]
			public List<HashPair> HashPairs { get; set; }
		}

		public class HashPair
		{
			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }

			[JsonPropertyName("saltLen")]
			public int? SaltLength { get; set; }
		}
	}
}
