using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.RSA
{
	public class RSAKeyGen186_2 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("modLength")]
		public List<int> ModLengths { get; private set; } = new List<int>();

		[JsonProperty("pubKeyValue")]
		public List<int> PublicKeyValues { get; private set; } = new List<int>();

		public RSAKeyGen186_2(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "keyGen", "186-2")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_1")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("FIPS186_3_RSA_Prerequisite_SHA_3")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("FIPS186_3_RSA_Prerequisite_DRBG_1")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("FIPS186_3_RSA_Prerequisite_DRBG_2")));


			//Mod Lengths
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod1024") == "True") ModLengths.Add(1024);
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod1536") == "True") ModLengths.Add(1536);
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod2048") == "True") ModLengths.Add(2048);
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod3072") == "True") ModLengths.Add(3072);
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_Mod4096") == "True") ModLengths.Add(4096);

			//Public Key values
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_E3") == "True") PublicKeyValues.Add(3);
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_E17") == "True") PublicKeyValues.Add(17);
			if (options.GetValue("REVALONLY_FIPS186_2KeyGen_E65537") == "True") PublicKeyValues.Add(65537);
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.RSAKeyGen186_2
		{
			ModLengths = ModLengths,
			PublicKeyValues = PublicKeyValues
		};
	}
}
