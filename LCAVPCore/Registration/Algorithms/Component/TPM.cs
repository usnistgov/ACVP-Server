using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class TPM : AlgorithmBase, IAlgorithm
	{
		public TPM(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "kdf-components", "tpm")
		{
			//There is nothing in this registration except prereqs...

			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("KDF_800_135_Prerequisite_SHA")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KDF_800_135_Prerequisite_HMAC")));
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KDF_TPM
		{
			
		};
	}
}
