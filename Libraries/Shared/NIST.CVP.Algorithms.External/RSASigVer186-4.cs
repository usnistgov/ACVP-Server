﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NIST.CVP.Algorithms.External
{
	public class RSASigVer186_4 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("conformances")]
		public List<string> Conformances { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		[JsonPropertyName("pubExpMode")]
		public string PublicExponentMode { get; set; }

		[JsonPropertyName("fixedPubExp")]
		public string FixedPublicExponent { get; set; }

		public RSASigVer186_4()
		{
			Name = "RSA";
			Mode = "sigVer";
			Revision = "1.0";
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