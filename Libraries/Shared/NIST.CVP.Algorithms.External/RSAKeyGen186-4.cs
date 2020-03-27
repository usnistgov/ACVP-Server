using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class RSAKeyGen186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		[JsonPropertyName("infoGeneratedByServer")]
		public bool InfoGeneratedByServer { get; set; }

		[JsonPropertyName("pubExpMode")]
		public string PublicExponentMode { get; set; }

		[JsonPropertyName("fixedPubExp")]
		public string FixedPublicExponent { get; set; }

		[JsonPropertyName("keyFormat")]
		public string KeyFormat { get; set; }

		public RSAKeyGen186_4()
		{
			Name = "RSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("randPQ")]
			public string RandomPQ { get; set; }

			[JsonPropertyName("properties")]
			public List<Property> Properties { get; set; }
		}

		public class Property
		{
			[JsonPropertyName("modulo")]
			public int Modulo { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }

			[JsonPropertyName("primeTest")]
			public List<string> PrimeTest { get; set; }
		}
	}
}
