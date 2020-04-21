using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.RSA
{
	public class RSAKeyGen : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("pubExpMode", NullValueHandling = NullValueHandling.Ignore)]
		public string PublicKeyExponentType { get; set; }

		[JsonProperty("fixedPubExp", NullValueHandling = NullValueHandling.Ignore)]
		public string FixedPublicKeyExponentValue { get; set; } = null;

		[JsonProperty("infoGeneratedByServer")]
		public bool InfoGeneratedByServer { get; set; }

		[JsonProperty("keyFormat")]
		public string KeyFormat { get => "standard"; }

		[JsonProperty("capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public RSAKeyGen(string publicKeyExpoonentType, string fixedEValue, Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "keyGen")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_3")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("FIPS186_3_RSA_Prerequisite_DRBG_1")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("FIPS186_3_RSA_Prerequisite_DRBG_2")));

			PublicKeyExponentType = publicKeyExpoonentType;

			//Add the value, but strip off any hex prefixes to get it to [0-9a-fA-F]. Not foolproof, but good enough for now
			FixedPublicKeyExponentValue = fixedEValue?.Replace("0x", "")?.Replace("0X", "")?.Replace("x", "")?.Replace("X", "");

			////Public Key Exponent type & value (if fixed)
			//if (options.GetValue("RSA2_Fixed_e") == "True")
			//{
			//	PublicKeyExponentType = "fixed";
			//	FixedPublicKeyExponentValue = options.GetValue("RSA2_Fixed_e_Value");
			//}
			//else
			//{
			//	PublicKeyExponentType = "random";
			//}


			InfoGeneratedByServer = options.GetValue("RSA2_NewFormat") == "True";


			//Probable
			if (options.GetValue("FIPS186_3KeyGen_ProbRP") == "True")
			{
				Capability capability = new Capability();
				capability.RandomPQSection = "B.3.3";

				//Mod 2048
				if (options.GetValue("RSA2_ProbRP_Mod2048") == "True")
				{
					Property cap = new Property();
					cap.Modulo = 2048;

					cap.PrimeTest = new List<string>();
					if (options.GetValue("RSA2_ProbRP_TableC2") == "True") cap.PrimeTest.Add("tblC2");
					if (options.GetValue("RSA2_ProbRP_TableC3") == "True") cap.PrimeTest.Add("tblC3");

					capability.Properties.Add(cap);
				}

				//Mod 3072
				if (options.GetValue("RSA2_ProbRP_Mod3072") == "True")
				{
					Property cap = new Property();
					cap.Modulo = 3072;

					cap.PrimeTest = new List<string>();
					if (options.GetValue("RSA2_ProbRP_TableC2") == "True") cap.PrimeTest.Add("tblC2");
					if (options.GetValue("RSA2_ProbRP_TableC3") == "True") cap.PrimeTest.Add("tblC3");

					capability.Properties.Add(cap);
				}

				if (capability.Properties.Count > 0)
				{
					Capabilities.Add(capability);
				}
			}

			//Provable
			if (options.GetValue("FIPS186_3KeyGen_ProvRP") == "True")
			{
				Capability capability = new Capability();
				capability.RandomPQSection = "B.3.2";


				//Mod 2048
				Property cap = new Property();
				cap.Modulo = 2048;

				cap.HashAlgorithms = new List<string>();
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA1") == "True") cap.HashAlgorithms.Add("SHA-1");
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA224") == "True") cap.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA256") == "True") cap.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA384") == "True") cap.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA512") == "True") cap.HashAlgorithms.Add("SHA2-512");
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA512224") == "True") cap.HashAlgorithms.Add("SHA2-512/224");
				if (options.GetValue("RSA2_ProvRP_Mod2048SHA512256") == "True") cap.HashAlgorithms.Add("SHA2-512/256");

				if (cap.HashAlgorithms.Count > 0)
				{
					capability.Properties.Add(cap);
				}

				//Mod 3072
				cap = new Property();
				cap.Modulo = 3072;

				cap.HashAlgorithms = new List<string>();
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA1") == "True") cap.HashAlgorithms.Add("SHA-1");
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA224") == "True") cap.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA256") == "True") cap.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA384") == "True") cap.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA512") == "True") cap.HashAlgorithms.Add("SHA2-512");
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA512224") == "True") cap.HashAlgorithms.Add("SHA2-512/224");
				if (options.GetValue("RSA2_ProvRP_Mod3072SHA512256") == "True") cap.HashAlgorithms.Add("SHA2-512/256");

				if (cap.HashAlgorithms.Count > 0)
				{
					capability.Properties.Add(cap);
				}

				if (capability.Properties.Count > 0)
				{
					Capabilities.Add(capability);
				}
			}

			//Provable with Conditions
			if (options.GetValue("FIPS186_3KeyGen_ProvPC") == "True")
			{
				Capability capability = new Capability();
				capability.RandomPQSection = "B.3.4";

				//Mod 2048
				Property cap = new Property();
				cap.Modulo = 2048;

				cap.HashAlgorithms = new List<string>();
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA1") == "True") cap.HashAlgorithms.Add("SHA-1");
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA224") == "True") cap.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA256") == "True") cap.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA384") == "True") cap.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA512") == "True") cap.HashAlgorithms.Add("SHA2-512");
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA512224") == "True") cap.HashAlgorithms.Add("SHA2-512/224");
				if (options.GetValue("RSA2_ProvPC_Mod2048SHA512256") == "True") cap.HashAlgorithms.Add("SHA2-512/256");

				if (cap.HashAlgorithms.Count > 0)
				{
					capability.Properties.Add(cap);
				}

				//Mod 3072
				cap = new Property();
				cap.Modulo = 3072;

				cap.HashAlgorithms = new List<string>();
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA1") == "True") cap.HashAlgorithms.Add("SHA-1");
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA224") == "True") cap.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA256") == "True") cap.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA384") == "True") cap.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA512") == "True") cap.HashAlgorithms.Add("SHA2-512");
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA512224") == "True") cap.HashAlgorithms.Add("SHA2-512/224");
				if (options.GetValue("RSA2_ProvPC_Mod3072SHA512256") == "True") cap.HashAlgorithms.Add("SHA2-512/256");

				if (cap.HashAlgorithms.Count > 0)
				{
					capability.Properties.Add(cap);
				}

				if (capability.Properties.Count > 0)
				{
					Capabilities.Add(capability);
				}
			}

			//Probable and Provable with Conditions
			if (options.GetValue("FIPS186_3KeyGen_BothPC") == "True")
			{
				Capability capability = new Capability();
				capability.RandomPQSection = "B.3.5";

				//Mod 2048
				Property cap = new Property();
				cap.Modulo = 2048;

				cap.HashAlgorithms = new List<string>();
				if (options.GetValue("RSA2_BothPC_Mod2048SHA1") == "True") cap.HashAlgorithms.Add("SHA-1");
				if (options.GetValue("RSA2_BothPC_Mod2048SHA224") == "True") cap.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("RSA2_BothPC_Mod2048SHA256") == "True") cap.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("RSA2_BothPC_Mod2048SHA384") == "True") cap.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("RSA2_BothPC_Mod2048SHA512") == "True") cap.HashAlgorithms.Add("SHA2-512");
				if (options.GetValue("RSA2_BothPC_Mod2048SHA512224") == "True") cap.HashAlgorithms.Add("SHA2-512/224");
				if (options.GetValue("RSA2_BothPC_Mod2048SHA512256") == "True") cap.HashAlgorithms.Add("SHA2-512/256");

				cap.PrimeTest = new List<string>();
				if (options.GetValue("RSA2_BothPC_TableC2") == "True") cap.PrimeTest.Add("tblC2");
				if (options.GetValue("RSA2_BothPC_TableC3") == "True") cap.PrimeTest.Add("tblC3");

				if (cap.HashAlgorithms.Count > 0)
				{
					capability.Properties.Add(cap);
				}

				//Mod 3072
				cap = new Property();
				cap.Modulo = 3072;

				cap.HashAlgorithms = new List<string>();
				if (options.GetValue("RSA2_BothPC_Mod3072SHA1") == "True") cap.HashAlgorithms.Add("SHA-1");
				if (options.GetValue("RSA2_BothPC_Mod3072SHA224") == "True") cap.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("RSA2_BothPC_Mod3072SHA256") == "True") cap.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("RSA2_BothPC_Mod3072SHA384") == "True") cap.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("RSA2_BothPC_Mod3072SHA512") == "True") cap.HashAlgorithms.Add("SHA2-512");
				if (options.GetValue("RSA2_BothPC_Mod3072SHA512224") == "True") cap.HashAlgorithms.Add("SHA2-512/224");
				if (options.GetValue("RSA2_BothPC_Mod3072SHA512256") == "True") cap.HashAlgorithms.Add("SHA2-512/256");

				cap.PrimeTest = new List<string>();
				if (options.GetValue("RSA2_BothPC_TableC2") == "True") cap.PrimeTest.Add("tblC2");
				if (options.GetValue("RSA2_BothPC_TableC3") == "True") cap.PrimeTest.Add("tblC3");

				if (cap.HashAlgorithms.Count > 0)
				{
					capability.Properties.Add(cap);
				}

				if (capability.Properties.Count > 0)
				{
					Capabilities.Add(capability);
				}
			}

			//Probable with Conditions
			if (options.GetValue("FIPS186_3KeyGen_ProbPC") == "True")
			{
				Capability capability = new Capability();
				capability.RandomPQSection = "B.3.6";

				//Mod 2048
				if (options.GetValue("RSA2_ProbPC_Mod2048") == "True")
				{
					Property cap = new Property();
					cap.Modulo = 2048;

					cap.PrimeTest = new List<string>();
					if (options.GetValue("RSA2_ProbPC_TableC2") == "True") cap.PrimeTest.Add("tblC2");
					if (options.GetValue("RSA2_ProbPC_TableC3") == "True") cap.PrimeTest.Add("tblC3");

					capability.Properties.Add(cap);
				}

				//Mod 3072
				if (options.GetValue("RSA2_ProbPC_Mod3072") == "True")
				{
					Property cap = new Property();
					cap.Modulo = 3072;

					cap.PrimeTest = new List<string>();
					if (options.GetValue("RSA2_ProbPC_TableC2") == "True") cap.PrimeTest.Add("tblC2");
					if (options.GetValue("RSA2_ProbPC_TableC3") == "True") cap.PrimeTest.Add("tblC3");

					capability.Properties.Add(cap);
				}

				if (capability.Properties.Count > 0)
				{
					Capabilities.Add(capability);
				}
			}

			if (Capabilities.Count == 0)
			{
				Errors.Add("RSA KeyGen was selected but child data was not properly populated");
			}
		}


		public class Capability
		{
			[JsonProperty("randPQ", NullValueHandling = NullValueHandling.Ignore)]
			public string RandomPQSection { get; set; }

			[JsonProperty("properties")]
			public List<Property> Properties { get; set; } = new List<Property>();
		}



		public class Property
		{
			[JsonProperty("modulo")]
			public int Modulo { get; set; }

			[JsonProperty("hashAlg", NullValueHandling = NullValueHandling.Ignore)]
			public List<string> HashAlgorithms { get; set; } = null;

			[JsonProperty("primeTest", NullValueHandling = NullValueHandling.Ignore)]
			public List<string> PrimeTest { get; set; } = null;
		}


		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.RSAKeyGen186_4
		{
			InfoGeneratedByServer = InfoGeneratedByServer,
			PublicExponentMode = PublicKeyExponentType,
			FixedPublicExponent = FixedPublicKeyExponentValue,
			KeyFormat = KeyFormat,
			Capabilities = Capabilities.Select(x => new RSAKeyGen186_4.Capability
			{
				RandomPQ = x.RandomPQSection,
				Properties = x.Properties.Select(p => new RSAKeyGen186_4.Property
				{
					Modulo = p.Modulo,
					HashAlgorithms = p.HashAlgorithms,
					PrimeTest = p.PrimeTest
				}).ToList()
			}).ToList()
		};
	}
}
