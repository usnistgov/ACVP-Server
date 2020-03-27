using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.ECDSA
{
	public class ECDSA_KeyPair : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "curve")]
		public List<string> Curves = new List<string>();

		[JsonProperty(PropertyName = "secretGenerationMode")]
		public List<string> SecretGenerationMode = new List<string>();

		public ECDSA_KeyPair(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ECDSA", "keyGen")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("ECDSA2_Prerequisite_DRBG")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("ECDSA2_Prerequisite_DRBG2")));

			string[] curves = new string[] { "P-224", "P-256", "P-384", "P-521", "K-233", "K-283", "K-409", "K-571", "B-233", "B-283", "B-409", "B-571" };

			foreach (string curve in curves)
			{
				if (options.GetValue($"KeyPair_{curve}") == "True") Curves.Add(curve);
			}

			if (options.GetValue("KeyPair_ExtraRandomBits") == "True") SecretGenerationMode.Add("extra bits");
			if (options.GetValue("KeyPair_TestingCandidates") == "True") SecretGenerationMode.Add("testing candidates");
			//if (options.GetValue("REVALONLY_KeyPair_FIPS_186_2") == "True") keyPair.Options.Add("186-2");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.ECDSAKeyGen186_4
		{
			Curves = Curves,
			SecretGenerationMode = SecretGenerationMode
		};
	}
}
