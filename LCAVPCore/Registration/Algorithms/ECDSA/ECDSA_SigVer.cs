using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.Algorithms.ECDSA.Capabilities;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.ECDSA
{
	public class ECDSA_SigVer : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "componentTest")]
		public bool IsComponent { get; } = false;

		[JsonProperty(PropertyName = "capabilities")]
		public List<SigCapability> Capabilities { get; private set; } = new List<SigCapability>();

		public ECDSA_SigVer(Dictionary<string, string> options) : base("ECDSA", "sigVer")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("ECDSA2_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("ECDSA2_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("ECDSA2_Prerequisite_SHA_3")));

			string[] curves = new string[] { "P-192", "P-224", "P-256", "P-384", "P-521", "K-163", "K-233", "K-283", "K-409", "K-571", "B-163", "B-233", "B-283", "B-409", "B-571" };

			//Though this could be done more succinctly, going to group by curve for simplicity
			foreach (string curve in curves)
			{
				//Get the hash algs for this curve
				List<string> hashAlgsForCurve = GetHashesForCurve(curve, options);

				//Add a capability if there are any hash algs
				if (hashAlgsForCurve.Count > 0)
				{
					Capabilities.Add(new SigCapability
					{
						Curve = new List<string> { curve },
						HashAlgorithms = hashAlgsForCurve
					});
				}
			}
		}

		private List<string> GetHashesForCurve(string curve, Dictionary<string, string> options)
		{
			List<string> hashAlgsForCurve = new List<string>();

			//Add all the SHAs that are enabled for this curve
			if (options.GetValue($"SigVer_{curve}_SHA-1") == "True") hashAlgsForCurve.Add("SHA-1");
			if (options.GetValue($"SigVer_{curve}_SHA-224") == "True") hashAlgsForCurve.Add("SHA2-224");
			if (options.GetValue($"SigVer_{curve}_SHA-256") == "True") hashAlgsForCurve.Add("SHA2-256");
			if (options.GetValue($"SigVer_{curve}_SHA-384") == "True") hashAlgsForCurve.Add("SHA2-384");
			if (options.GetValue($"SigVer_{curve}_SHA-512") == "True") hashAlgsForCurve.Add("SHA2-512");
			if (options.GetValue($"SigVer_{curve}_SHA-512224") == "True") hashAlgsForCurve.Add("SHA2-512/224");
			if (options.GetValue($"SigVer_{curve}_SHA-512256") == "True") hashAlgsForCurve.Add("SHA2-512/256");

			return hashAlgsForCurve;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.ECDSASigVer186_4
		{
			ComponentTest = false,
			Capabilities = Capabilities.Select(x => new ECDSASigVer186_4.Capability
			{
				Curves = x.Curve,
				HashAlgorithms = x.HashAlgorithms
			}).ToList()
		};
	}
}
