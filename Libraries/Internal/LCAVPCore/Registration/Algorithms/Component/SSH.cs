using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class SSH : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "cipher")]
		public List<string> Ciphers { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "hashAlg")]
		public List<string> HashAlgorithms { get; private set; } = new List<string>();

		public SSH(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "ssh")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));

			if (options.GetValue("KDF_800_135_SSH_TDES") == "True") Ciphers.Add("TDES");
			if (options.GetValue("KDF_800_135_SSH_AES_128") == "True") Ciphers.Add("AES-128");
			if (options.GetValue("KDF_800_135_SSH_AES_192") == "True") Ciphers.Add("AES-192");
			if (options.GetValue("KDF_800_135_SSH_AES_256") == "True") Ciphers.Add("AES-256");

			if (options.GetValue("KDF_800_135_SSH_SHA_1") == "True") HashAlgorithms.Add("SHA-1");
			if (options.GetValue("KDF_800_135_SSH_SHA_224") == "True") HashAlgorithms.Add("SHA2-224");
			if (options.GetValue("KDF_800_135_SSH_SHA_256") == "True") HashAlgorithms.Add("SHA2-256");
			if (options.GetValue("KDF_800_135_SSH_SHA_384") == "True") HashAlgorithms.Add("SHA2-384");
			if (options.GetValue("KDF_800_135_SSH_SHA_512") == "True") HashAlgorithms.Add("SHA2-512");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.KDF_SSH
		{
			Ciphers = Ciphers,
			HashAlgorithms = HashAlgorithms
		};
	}
}
