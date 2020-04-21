using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.DSA.Capabilities;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.DSA
{
	public class DSAPQGGen : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<PQGGenCapability> Capabilities { get; private set; } = new List<PQGGenCapability>();

		public DSAPQGGen(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "DSA", "pqgGen")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_2")));
			//PreReqs.Add(BuildPrereq("DRBG", options.GetValue("DSA2_Prerequisite_DRBG")));

			//Though the PQGen value is global, new model wants it inside the capability, so get it now and use later
			List<string> pqGen = new List<string>();
			if (options.GetValue("PQGGen_ProbablePrimePQ") == "True") pqGen.Add("probable");
			if (options.GetValue("PQGGen_ProvablePrimePQ") == "True") pqGen.Add("provable");

			//Same with the G gen values, also the same across all
			List<string> gGen = new List<string>();
			if (options.GetValue("PQGGen_UnverifiableG") == "True") gGen.Add("unverifiable");
			if (options.GetValue("PQGGen_CanonicalG") == "True") gGen.Add("canonical");

			//Do the 3 L/N pairs
			//L=2048, N=224
			List<string> shas = new List<string>();
			if (options.GetValue("PQGGen_L2048N224_SHA-224") == "True") shas.Add("SHA2-224");
			if (options.GetValue("PQGGen_L2048N224_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGGen_L2048N224_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGGen_L2048N224_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGGen_L2048N224_SHA-512224") == "True") shas.Add("SHA2-512/224");
			if (options.GetValue("PQGGen_L2048N224_SHA-512256") == "True") shas.Add("SHA2-512/256");
			if (shas.Count > 0)
			{
				Capabilities.Add(new PQGGenCapability
				{
					L = 2048,
					N = 224,
					HashAlgorithms = shas,
					PQGen = pqGen,
					GGen = gGen
				});
			}

			//L=2048, N=256
			shas = new List<string>();
			if (options.GetValue("PQGGen_L2048N256_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGGen_L2048N256_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGGen_L2048N256_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGGen_L2048N256_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new PQGGenCapability
				{
					L = 2048,
					N = 256,
					HashAlgorithms = shas,
					PQGen = pqGen,
					GGen = gGen
				});
			}

			//L=3072, N=256
			shas = new List<string>();
			if (options.GetValue("PQGGen_L3072N256_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGGen_L3072N256_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGGen_L3072N256_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGGen_L3072N256_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new PQGGenCapability
				{
					L = 3072,
					N = 256,
					HashAlgorithms = shas,
					PQGen = pqGen,
					GGen = gGen
				});
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.DSAPQGGen186_4
		{
			Capabilities = Capabilities.Select(x => new DSAPQGGen186_4.Capability
			{
				PQGen = x.PQGen,
				GGen = x.GGen,
				L = x.L,
				N = x.N,
				HashAlgorithms = x.HashAlgorithms
			}).ToList()
		};
	}
}
