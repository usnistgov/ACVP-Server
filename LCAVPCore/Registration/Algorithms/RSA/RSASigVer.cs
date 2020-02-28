using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.RSA
{
	public class RSASigVer : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("pubExpMode", NullValueHandling = NullValueHandling.Ignore)]
		public string PublicKeyExponentType { get; set; }

		[JsonProperty("fixedPubExp", NullValueHandling = NullValueHandling.Ignore)]
		public string FixedPublicKeyExponentValue { get; set; } = null;

		[JsonProperty("capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public RSASigVer(string publicKeyExponentType, string fixedEValue, Dictionary<string, string> options) : base("RSA", "sigVer")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_3")));

			//Handle the variety of ways that e can be done (or at least specified in CAVS), relying on the caller to do the logic
			PublicKeyExponentType = publicKeyExponentType;
			FixedPublicKeyExponentValue = fixedEValue;

			//As we go through the 3 signature types, we have to make sure that this sig type has the right public key exponent options - random, fixed with no value, fixed with that specific value

			//ANSX 9.31
			if (options.GetValue("FIPS186_3SigVer") == "True" &&
				((publicKeyExponentType == "random" && options.GetValue("FIPS186_3SigVer_Random_e_Value") == "True") ||
				 (publicKeyExponentType == "fixed" && options.GetValue("FIPS186_3SigVer_Fixed_e_Value") == "True" && ((fixedEValue != null && (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Min")) == fixedEValue || ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Max")) == fixedEValue)) ||
																														(fixedEValue == null && ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Min")) == null && ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVer_Fixed_e_Value_Max")) == null)))
				)
			   )
			{
				Capability algSpecs = new Capability();
				algSpecs.SigType = "ansx9.31";

				//Mod 1024
				if (options.GetValue("FIPS186_3SigVer_mod1024") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 1024;
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigVer_mod1024SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					algSpecs.Properties.Add(cst);
				}

				//Mod 2048
				if (options.GetValue("FIPS186_3SigVer_mod2048") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 2048;
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigVer_mod2048SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					algSpecs.Properties.Add(cst);
				}

				//Mod 3072
				if (options.GetValue("FIPS186_3SigVer_mod3072") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 3072;
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigVer_mod3072SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					algSpecs.Properties.Add(cst);
				}

				//Though the UI has 4096, it is not in the inf file

				Capabilities.Add(algSpecs);
			}

			//PKCS1.5
			if (options.GetValue("FIPS186_3SigVerPKCS15") == "True" &&
				((publicKeyExponentType == "random" && options.GetValue("FIPS186_3SigVerPKCS15_Random_e_Value") == "True") ||
				 (publicKeyExponentType == "fixed" && options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value") == "True" && ((fixedEValue != null && (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Min")) == fixedEValue || ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Max")) == fixedEValue)) ||
																														(fixedEValue == null && ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Min")) == null && ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCS15_Fixed_e_Value_Max")) == null)))
				)
			   )
			{
				Capability algSpecs = new Capability();
				algSpecs.SigType = "pkcs1v1.5";

				//Mod 1024
				if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 1024;
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod1024SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					algSpecs.Properties.Add(cst);
				}

				//Mod 2048
				if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 2048;
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod2048SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					algSpecs.Properties.Add(cst);
				}

				//Mod 3072
				if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 3072;
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224" });
					if (options.GetValue("FIPS186_3SigVerPKCS15_mod3072SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256" });
					algSpecs.Properties.Add(cst);
				}

				//Though the UI has 4096, it is not in the inf file

				Capabilities.Add(algSpecs);
			}

			//PSS
			if (options.GetValue("FIPS186_3SigVerPKCSPSS") == "True" &&
				((publicKeyExponentType == "random" && options.GetValue("FIPS186_3SigVerPKCSPSS_Random_e_Value") == "True") ||
				 (publicKeyExponentType == "fixed" && options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value") == "True" && ((fixedEValue != null && (ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Min")) == fixedEValue || ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Max")) == fixedEValue)) ||
																																(fixedEValue == null && ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Min")) == null && ParsingHelper.ZeroStringToNull(options.GetValue("FIPS186_3SigVerPKCSPSS_Fixed_e_Value_Max")) == null)))
				)
			   )
			{
				Capability algSpecs = new Capability();
				algSpecs.SigType = "pss";

				//Mod 1024
				if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 1024;
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA1SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA224SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA256SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA384SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512224SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod1024SHA512256SaltLen")) });
					algSpecs.Properties.Add(cst);
				}

				//Mod 2048
				if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 2048;
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA1SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA224SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA256SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA384SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512224SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod2048SHA512256SaltLen")) });
					algSpecs.Properties.Add(cst);
				}

				//Mod 3072
				if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072") == "True")
				{
					Property cst = new Property();
					cst.Modulo = 3072;
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA1") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA-1", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA1SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA224SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA256SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA384") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-384", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA384SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512224") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/224", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512224SaltLen")) });
					if (options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512256") == "True") cst.HashPairs.Add(new HashPair { HashAlgorithm = "SHA2-512/256", SaltLen = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("FIPS186_3SigVerPKCSPSS_mod3072SHA512256SaltLen")) });
					algSpecs.Properties.Add(cst);
				}

				//Though the UI has 4096, it is not in the inf file

				Capabilities.Add(algSpecs);
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

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.RSASigVer186_4
		{
			PublicExponentMode = PublicKeyExponentType,
			FixedPublicExponent = FixedPublicKeyExponentValue,
			Capabilities = Capabilities.Select(x => new RSASigVer186_4.Capability
			{
				SignatureType = x.SigType,
				Properties = x.Properties.Select(p => new RSASigVer186_4.Property
				{
					Modulo = p.Modulo,
					HashPairs = p.HashPairs.Select(h => new RSASigVer186_4.HashPair
					{
						HashAlgorithm = h.HashAlgorithm,
						SaltLength = h.SaltLen
					}).ToList()
				}).ToList()
			}).ToList()
		};
	}
}
