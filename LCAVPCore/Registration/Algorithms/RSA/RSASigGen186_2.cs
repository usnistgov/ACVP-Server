using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.RSA
{
	public class RSASigGen186_2 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public RSASigGen186_2(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "sigGen", "186-2")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_3")));

			//ANSX 9.31
			if (options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096") == "True")
			{
				Capability capability = new Capability();
				capability.SigType = "ansx9.31";

				if (options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA224") == "True") capability.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA256") == "True") capability.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA384") == "True") capability.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("REVALONLY_FIPS186_2SigGen_mod4096SHA512") == "True") capability.HashAlgorithms.Add("SHA2-512");

				Capabilities.Add(capability);
			}

			//PKCS1.5
			if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096") == "True")
			{
				Capability capability = new Capability();
				capability.SigType = "pkcs1v1.5";

				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA224") == "True") capability.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA256") == "True") capability.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA384") == "True") capability.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCS15_mod4096SHA512") == "True") capability.HashAlgorithms.Add("SHA2-512");

				Capabilities.Add(capability);
			}

			//PSS
			if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096") == "True")
			{
				Capability capability = new Capability();
				capability.SigType = "pss";

				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA224") == "True") capability.HashAlgorithms.Add("SHA2-224");
				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA256") == "True") capability.HashAlgorithms.Add("SHA2-256");
				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA384") == "True") capability.HashAlgorithms.Add("SHA2-384");
				if (options.GetValue("REVALONLY_FIPS186_2SigGenPKCSPSS_mod4096SHA512") == "True") capability.HashAlgorithms.Add("SHA2-512");

				Capabilities.Add(capability);
			}
		}

		public class Capability
		{
			[JsonProperty("sigType")]
			public string SigType { get; set; }

			[JsonProperty("modulo")]
			public List<int> Modulos { get; set; } = new List<int> { 4096 };        //This can only be 4096 for 186-2, but the model has it as an array

			[JsonProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; } = new List<string>();
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.RSASigGen186_2
		{
			Capabilities = Capabilities.Select(x => new ACVPCore.Algorithms.Persisted.RSASigGen186_2.Capability
			{
				SignatureType = x.SigType,
				Modulo = x.Modulos,
				HashAlgorithms = x.HashAlgorithms
			}).ToList()
		};
	}
}
