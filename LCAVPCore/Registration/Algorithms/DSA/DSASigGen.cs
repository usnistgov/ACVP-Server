using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.Algorithms.DSA.Capabilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.Registration.Algorithms.DSA
{
	public class DSASigGen : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<SigGenCapability> Capabilities { get; private set; } = new List<SigGenCapability>();

		public DSASigGen(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "DSA", "sigGen")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("DSA2_Prerequisite_DRBG")));

			//Do the 3 L/N pairs
			//L=2048, N=224
			List<string> shas = new List<string>();
			//if (options.GetValue("SigGen_L2048N224_SHA-1") == "True") shas.Add("SHA-1");
			if (options.GetValue("SigGen_L2048N224_SHA-224") == "True") shas.Add("SHA2-224");
			if (options.GetValue("SigGen_L2048N224_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("SigGen_L2048N224_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("SigGen_L2048N224_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("SigGen_L2048N224_SHA-512224") == "True") shas.Add("SHA2-512/224");
			if (options.GetValue("SigGen_L2048N224_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new SigGenCapability { L = 2048, N = 224, HashAlgorithms = shas });
			}

			//L=2048, N=256
			shas = new List<string>();
			//if (options.GetValue("SigGen_L2048N256_SHA-1") == "True") shas.Add("SHA-1");
			if (options.GetValue("SigGen_L2048N256_SHA-224") == "True") shas.Add("SHA2-224");
			if (options.GetValue("SigGen_L2048N256_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("SigGen_L2048N256_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("SigGen_L2048N256_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("SigGen_L2048N256_SHA-512224") == "True") shas.Add("SHA2-512/224");
			if (options.GetValue("SigGen_L2048N256_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new SigGenCapability { L = 2048, N = 256, HashAlgorithms = shas });
			}

			//L=3072, N=256
			shas = new List<string>();
			//if (options.GetValue("SigGen_L3072N256_SHA-1") == "True") shas.Add("SHA-1");
			if (options.GetValue("SigGen_L3072N256_SHA-224") == "True") shas.Add("SHA2-224");
			if (options.GetValue("SigGen_L3072N256_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("SigGen_L3072N256_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("SigGen_L3072N256_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("SigGen_L3072N256_SHA-512224") == "True") shas.Add("SHA2-512/224");
			if (options.GetValue("SigGen_L3072N256_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new SigGenCapability { L = 3072, N = 256, HashAlgorithms = shas });
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.DSASigGen186_4
		{
			Capabilities = Capabilities.Select(x => new DSASigGen186_4.Capability
			{
				L = x.L,
				N = x.N,
				HashAlgorithms = x.HashAlgorithms
			}).ToList()
		};
	}
}
