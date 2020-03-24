using System.Collections.Generic;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class RSAKeyGen186_4 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		[AlgorithmProperty("infoGeneratedByServer")]
		public bool InfoGeneratedByServer { get; set; }

		[AlgorithmProperty("pubExpMode")]
		public string PublicExponentMode { get; set; }

		[AlgorithmProperty("fixedPubExp")]
		public string FixedPublicExponent { get; set; }

		[AlgorithmProperty("keyFormat")]
		public string KeyFormat { get; set; }

		public RSAKeyGen186_4()
		{
			Name = "RSA";
			Mode = "keyGen";
			Revision = "1.0";
		}

		public RSAKeyGen186_4(External.RSAKeyGen186_4 external) : this()
		{
			InfoGeneratedByServer = external.InfoGeneratedByServer;
			PublicExponentMode = external.PublicExponentMode;
			FixedPublicExponent = external.FixedPublicExponent;
			KeyFormat = external.KeyFormat;

			foreach (var externalCapability in external.Capabilities)
			{
				Capabilities.Add(new Capability(externalCapability));
			}
		}

		public class Capability
		{
			[AlgorithmProperty("randPQ")]
			public string RandomPQ { get; set; }

			[AlgorithmProperty("properties")]
			public List<Property> Properties { get; set; } = new List<Property>();

			public Capability() { }

			public Capability(External.RSAKeyGen186_4.Capability externalCapability)
			{
				RandomPQ = externalCapability.RandomPQ;

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

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }

			[AlgorithmProperty("primeTest")]
			public List<string> PrimeTest { get; set; }

			public Property() { }

			public Property(External.RSAKeyGen186_4.Property externalProperty)
			{
				Modulo = externalProperty.Modulo;
				HashAlgorithms = externalProperty.HashAlgorithms;
				PrimeTest = externalProperty.PrimeTest;
			}
		}
	}
}
