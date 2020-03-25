using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Algorithms.Persisted;
using LCAVPCore.Registration.Algorithms.DSA.Capabilities;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.DSA
{
	public class DSAPQGVer : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<PQGVerCapability> Capabilities { get; private set; } = new List<PQGVerCapability>();

		public DSAPQGVer(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "DSA", "pqgVer")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_2")));
			//PreReqs.Add(BuildPrereq("DRBG", options.GetValue("DSA2_Prerequisite_DRBG")));

			//If they checked the 186-2 box it implies certain selections that may not be reflected in the other fields, so create a special capability for it
			if (options.GetValue("PQGVer_FIPS186-2PQGVerTest") == "True")
			{
				Capabilities.Add(new PQGVerCapability
				{
					L = 1024,
					N = 160,
					HashAlgorithms = new List<string> { "SHA-1" },
					PQGen = new List<string> { "probable" },
					GGen = new List<string> { "unverifiable" }
				});
			}

			//Though the PQGen value is global, new model wants it inside the capability, so get it now and use later
			List<string> pqGen = new List<string>();
			if (options.GetValue("PQGVer_ProbablePrimePQ") == "True") pqGen.Add("probable");
			if (options.GetValue("PQGVer_ProvablePrimePQ") == "True") pqGen.Add("provable");

			//Same with the G gen values, also the same across all
			List<string> gGen = new List<string>();
			if (options.GetValue("PQGVer_UnverifiableG") == "True") gGen.Add("unverifiable");
			if (options.GetValue("PQGVer_CanonicalG") == "True") gGen.Add("canonical");

			//Do the 4 L/N pairs
			//L=1024, N=160
			List<string> shas = new List<string>();
			if (options.GetValue("PQGVer_L1024N160_SHA-1") == "True") shas.Add("SHA-1");
			if (options.GetValue("PQGVer_L1024N160_SHA-224") == "True") shas.Add("SHA2-224");
			if (options.GetValue("PQGVer_L1024N160_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGVer_L1024N160_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGVer_L1024N160_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGVer_L1024N160_SHA-512224") == "True") shas.Add("SHA2-512/224");
			if (options.GetValue("PQGVer_L1024N160_SHA-512256") == "True") shas.Add("SHA2-512/256");
			if (shas.Count > 0)
			{
				//Doublecheck for the special 186-2 case... like maybe they checked some or all of the boxes that are implied by the 186-2 box. If they checked 186-2, then didn't check anything that isn't implied by that, then don't create
				if (!(options.GetValue("PQGVer_FIPS186-2PQGVerTest") == "True" && (shas.Count == 0 || (shas.Count == 1 && shas[0] == "SHA-1")) && (pqGen.Count == 0 || (pqGen.Count == 1 && pqGen[0] == "probable")) && (gGen.Count == 0 || (gGen.Count == 1 && gGen[0] == "unverifiable"))))
				{
					Capabilities.Add(new PQGVerCapability
					{
						L = 1024,
						N = 160,
						HashAlgorithms = shas,
						PQGen = pqGen,
						GGen = gGen
					});
				}
			}

			//L=2048, N=224
			shas = new List<string>();
			if (options.GetValue("PQGVer_L2048N224_SHA-224") == "True") shas.Add("SHA2-224");
			if (options.GetValue("PQGVer_L2048N224_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGVer_L2048N224_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGVer_L2048N224_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGVer_L2048N224_SHA-512224") == "True") shas.Add("SHA2-512/224");
			if (options.GetValue("PQGVer_L2048N224_SHA-512256") == "True") shas.Add("SHA2-512/256");
			if (shas.Count > 0)
			{
				Capabilities.Add(new PQGVerCapability
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
			if (options.GetValue("PQGVer_L2048N256_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGVer_L2048N256_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGVer_L2048N256_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGVer_L2048N256_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new PQGVerCapability
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
			if (options.GetValue("PQGVer_L3072N256_SHA-256") == "True") shas.Add("SHA2-256");
			if (options.GetValue("PQGVer_L3072N256_SHA-384") == "True") shas.Add("SHA2-384");
			if (options.GetValue("PQGVer_L3072N256_SHA-512") == "True") shas.Add("SHA2-512");
			if (options.GetValue("PQGVer_L3072N256_SHA-512256") == "True") shas.Add("SHA2-512/256");

			if (shas.Count > 0)
			{
				Capabilities.Add(new PQGVerCapability
				{
					L = 3072,
					N = 256,
					HashAlgorithms = shas,
					PQGen = pqGen,
					GGen = gGen
				});
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.DSAPQGVer186_4
		{
			Capabilities = Capabilities.Select(x => new DSAPQGVer186_4.Capability
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
