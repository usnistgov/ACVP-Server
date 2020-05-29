using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class RSASigGen186_5 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public RSASigGen186_5()
		{
			Name = "RSA";
			Mode = "sigGen";
			Revision = "FIPS186-5";
		}

		//TODO - this should account for SaltLength being required for PSS SignatureType, and not allowed for the others

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

			[JsonPropertyName("maskFunction")]
			public List<string> MaskFunctions { get; set; }

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
