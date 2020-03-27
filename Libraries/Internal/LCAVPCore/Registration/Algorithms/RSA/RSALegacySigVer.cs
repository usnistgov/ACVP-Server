using System;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.RSA
{
	public class RSALegacySigVer : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public RSALegacySigVer(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "legacySigVer")
		{
			//ANSX 9.31
			if (options.GetValue("Legacy_FIPS186_2SigVer") == "True")
			{
				Capability algSpecs = new Capability();
				algSpecs.SigType = "ansx9.31";

				//Get the hash pairs since they'll be the same for all modulos
				List<HashPair> hashPairs = new List<HashPair>();
				if (options.GetValue("Legacy_FIPS186_2SigVer_SHA1") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
				if (options.GetValue("Legacy_FIPS186_2SigVer_SHA224") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
				if (options.GetValue("Legacy_FIPS186_2SigVer_SHA256") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
				if (options.GetValue("Legacy_FIPS186_2SigVer_SHA384") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
				if (options.GetValue("Legacy_FIPS186_2SigVer_SHA512") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });

				foreach (int modulo in new int[] { 1024, 1536, 2048, 3072, 4096 })
				{
					if (options.GetValue($"Legacy_FIPS186_2SigVer_mod{modulo}") == "True")
					{
						algSpecs.Properties.Add(new Properties
						{
							Modulo = modulo,
							HashPairs = hashPairs
						});
					}
				}

				Capabilities.Add(algSpecs);
			}

			//PKCS1.5
			if (options.GetValue("Legacy_PKCS#1_15SigVer") == "True")
			{
				Capability algSpecs = new Capability();
				algSpecs.SigType = "pkcs1v1.5";

				//Get the hash pairs since they'll be the same for all modulos
				List<HashPair> hashPairs = new List<HashPair>();
				if (options.GetValue("Legacy_PKCS#1_15SigVer_SHA1") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
				if (options.GetValue("Legacy_PKCS#1_15SigVer_SHA224") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
				if (options.GetValue("Legacy_PKCS#1_15SigVer_SHA256") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
				if (options.GetValue("Legacy_PKCS#1_15SigVer_SHA384") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
				if (options.GetValue("Legacy_PKCS#1_15SigVer_SHA512") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });

				foreach (int modulo in new int[] { 1024, 1536, 2048, 3072, 4096 })
				{
					if (options.GetValue($"Legacy_PKCS#1_15SigVer_mod{modulo}") == "True")
					{
						algSpecs.Properties.Add(new Properties
						{
							Modulo = modulo,
							HashPairs = hashPairs
						});
					}
				}

				Capabilities.Add(algSpecs);
			}


			//PSS
			if (options.GetValue("Legacy_PKCS#1_PSSSigVer") == "True")
			{
				Capability algSpecs = new Capability();
				algSpecs.SigType = "pss";

				//Get the saltLen. We'll use it later
				int saltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("Legacy_PKCS#1_PSSSigVer_SaltLen"));

				//Get the hash pairs since they'll be the same for all modulos. CAVS only allows specification of a single salt length, but what is legal actually varies given the hashAlg, so we'll take the min of what they said and the max of what is legal
				List<HashPair> hashPairs = new List<HashPair>();
				if (options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA1") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA-1", SaltLen = Math.Min(saltLen, 160) });
				if (options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA224") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224", SaltLen = Math.Min(saltLen, 224) });
				if (options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA256") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256", SaltLen = Math.Min(saltLen, 256) });
				if (options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA384") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384", SaltLen = Math.Min(saltLen, 384) });
				if (options.GetValue("Legacy_PKCS#1_PSSSigVer_SHA512") == "True") hashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512", SaltLen = Math.Min(saltLen, 512) });

				foreach (int modulo in new int[] { 1024, 1536, 2048, 3072, 4096 })
				{
					if (options.GetValue($"Legacy_PKCS#1_PSSSigVer_mod{modulo}") == "True")
					{
						algSpecs.Properties.Add(new Properties
						{
							Modulo = modulo,
							HashPairs = hashPairs
						});
					}
				}

				Capabilities.Add(algSpecs);
			}
		}

		public class Capability
		{
			[JsonProperty("sigType")]
			public string SigType { get; set; }

			[JsonProperty("properties")]
			public List<Properties> Properties { get; set; } = new List<Properties>();
		}

		public class Properties
		{
			[JsonProperty("modulo")]
			public int Modulo { get; set; }

			[JsonProperty("hashPair")]
			public List<HashPair> HashPairs { get; set; } = new List<HashPair>();
		}

		public class HashPair
		{
			[JsonProperty("hashAlg")]
			public string HashAlgorithm { get; set; }

			[JsonProperty("saltLen", NullValueHandling = NullValueHandling.Ignore)]
			public int? SaltLen { get; set; } = null;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.RSASigVer186_2
		{
			Capabilities = Capabilities.Select(x => new NIST.CVP.Algorithms.Persisted.RSASigVer186_2.Capability
			{
				SignatureType = x.SigType,
				Properties = x.Properties.Select(p => new RSASigVer186_2.Property
				{
					Modulo = p.Modulo,
					HashPairs = p.HashPairs.Select(h => new RSASigVer186_2.HashPair
					{
						HashAlgorithm = h.HashAlgorithm,
						SaltLength = h.SaltLen
					}).ToList()
				}).ToList()
			}).ToList()
		};
	}
}
