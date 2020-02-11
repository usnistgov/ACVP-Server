using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class RSASigVer186_2 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("pubExpMode")]
		public string PublicExponentMode { get; set; }

		[AlgorithmProperty("fixedPubExp")]
		public string FixedPublicExponent { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public RSASigVer186_2()
		{
			Name = "RSA";
			Mode = "legacySigVer";
			Revision = "1.0";
		}

		public RSASigVer186_2(ACVPCore.Algorithms.External.RSASigVer186_2 external) : this()
		{
			PublicExponentMode = external.PublicExponentMode;
			FixedPublicExponent = external.FixedPublicExponent;

			foreach (var externalCapability in external.Capabilities)
			{
				Capabilities.Add(new Capability(externalCapability));
			}
		}

		public class Capability
		{
			[AlgorithmProperty("sigType")]
			public string SignatureType { get; set; }

			[AlgorithmProperty("properties")]
			public List<Property> Properties { get; set; } = new List<Property>();

			public Capability() { }

			public Capability(External.RSASigVer186_2.Capability externalCapability)
			{
				SignatureType = externalCapability.SignatureType;
				foreach (var externalProperty in externalCapability.Properties)
				{
					Properties.Add(new Property(externalProperty));
				}
			}
		}

		public class Property
		{
			[AlgorithmProperty("modulo")]
			public int Modulo { get; set; }

			[AlgorithmProperty("hashPair")]
			public List<HashPair> HashPairs { get; set; } = new List<HashPair>();

			public Property() { }

			public Property(External.RSASigVer186_2.Property externalProperty)
			{
				Modulo = externalProperty.Modulo;
				foreach (var externalHashPair in externalProperty.HashPairs)
				{
					HashPairs.Add(new HashPair(externalHashPair));
				}
			}
		}

		public class HashPair
		{
			[AlgorithmProperty("hashAlg")]
			public string HashAlgorithm { get; set; }

			[AlgorithmProperty("saltLen")]
			public int? SaltLength { get; set; }

			public HashPair() { }

			public HashPair(External.RSASigVer186_2.HashPair externalHashPair)
			{
				HashAlgorithm = externalHashPair.HashAlgorithm;
				SaltLength = externalHashPair.SaltLength;
			}
		}


	}
}
