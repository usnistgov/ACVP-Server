﻿using System.Collections.Generic;

namespace ACVPCore.Algorithms.Persisted
{
	public class RSASigGen186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("conformances")]
		public List<string> Conformances { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public RSASigGen186_4()
		{
			Name = "RSA";
			Mode = "sigGen";
			Revision = "1.0";
		}

		public RSASigGen186_4(ACVPCore.Algorithms.External.RSASigGen186_4 external) : this()
		{
			Conformances = external.Conformances;

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

			public Capability(External.RSASigGen186_4.Capability externalCapability)
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

			[AlgorithmProperty("primeTest")]
			public List<HashPair> HashPairs { get; set; } = new List<HashPair>();

			public Property() { }

			public Property(External.RSASigGen186_4.Property externalProperty)
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
			public List<string> HashAlgorithms { get; set; }

			[AlgorithmProperty("saltLen")]
			public int? SaltLength { get; set; }

			public HashPair() { }
			public HashPair(External.RSASigGen186_4.HashPair externalHashPair)
			{
				HashAlgorithms = externalHashPair.HashAlgorithms;
				SaltLength = externalHashPair.SaltLength;
			}
		}
	}
}