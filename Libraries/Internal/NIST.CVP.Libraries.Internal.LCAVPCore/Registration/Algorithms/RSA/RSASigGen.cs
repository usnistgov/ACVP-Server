using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.RSA
{
	public class RSASigGen : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		[JsonIgnore]
		public bool Have186_4Capabilities => Capabilities.Count > 0;

		public RSASigGen(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "sigGen")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_3")));

			//ANSX 9.31
			if (options.GetValue("FIPS186_3SigGen") == "True")
			{
				Capability capabilities = new Capability();
				capabilities.SigType = "ansx9.31";

				//Mod 2048
				if (options.GetValue("FIPS186_3SigGen_mod2048") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 2048;
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigGen_mod2048SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					capabilities.Properties.Add(cst);
				}

				//Mod 3072
				if (options.GetValue("FIPS186_3SigGen_mod3072") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 3072;
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigGen_mod3072SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					capabilities.Properties.Add(cst);
				}

				//Though the UI has 4096, it is not in the inf file - it is only for 186-2. Since CAVS says FIPS186_3xxx is true even if you only did 186-2 4096, need a special check that we actually have 186-4 capabilities
				if (capabilities.Properties.Count > 0)
				{
					Capabilities.Add(capabilities);
				}
			}

			//PKCS1.5
			if (options.GetValue("FIPS186_3SigGenPKCS15") == "True")
			{
				Capability capabilities = new Capability();
				capabilities.SigType = "pkcs1v1.5";

				//Mod 2048
				if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 2048;
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod2048SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					capabilities.Properties.Add(cst);
				}

				//Mod 3072
				if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 3072;
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigGenPKCS15_mod3072SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					capabilities.Properties.Add(cst);
				}

				//Though the UI has 4096, it is not in the inf file - it is only for 186-2. Since CAVS says FIPS186_3xxx is true even if you only did 186-2 4096, need a special check that we actually have 186-4 capabilities
				if (capabilities.Properties.Count > 0)
				{
					Capabilities.Add(capabilities);
				}
			}

			//PSS
			if (options.GetValue("FIPS186_3SigGenPKCSPSS") == "True")
			{
				Capability capabilities = new Capability();
				capabilities.SigType = "pss";

				//Mod 2048
				if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 2048;
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA1SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA224SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA256SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA384SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA512SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA512224SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod2048SHA512256SaltLen")) });
					capabilities.Properties.Add(cst);
				}

				//Mod 3072
				if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 3072;
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA1SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA224SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA256SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA384SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA512SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA512224SaltLen")) });
					if (options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigGenPKCSPSS_mod3072SHA512256SaltLen")) });
					capabilities.Properties.Add(cst);
				}

				//Though the UI has 4096, it is not in the inf file - it is only for 186-2. Since CAVS says FIPS186_3xxx is true even if you only did 186-2 4096, need a special check that we actually have 186-4 capabilities
				if (capabilities.Properties.Count > 0)
				{
					Capabilities.Add(capabilities);
				}
			}

		}

		public class Capability
		{
			[JsonProperty("sigType")]
			public string SigType { get; set; }

			[JsonProperty("properties")]
			public List<Property> Properties { get; set; } = new List<Property>();
		}

		public class Property
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

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.RSASigGen186_4
		{
			Capabilities = Capabilities.Select(x => new RSASigGen186_4.Capability
			{
				SignatureType = x.SigType,
				Properties = x.Properties.Select(p => new RSASigGen186_4.Property
				{
					Modulo = p.Modulo,
					HashPairs = p.HashPairs.Select(h => new RSASigGen186_4.HashPair { 
					HashAlgorithm = h.HashAlgorithm,
					SaltLength = h.SaltLen
					}).ToList()
				}).ToList()
			}).ToList()
		};
	}
}
