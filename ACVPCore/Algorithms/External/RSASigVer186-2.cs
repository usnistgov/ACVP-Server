using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class RSASigVer186_2 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("pubExpMode")]
		public string PublicExponentMode { get; set; }

		[JsonPropertyName("fixedPubExp")]
		public string FixedPublicExponent { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public RSASigVer186_2()
		{
			Name = "RSA";
			Mode = "legacySigVer";
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
			public string HashAlgorithm { get; set; }

			[JsonPropertyName("saltLen")]
			public int? SaltLength { get; set; }
		}
	}
}
