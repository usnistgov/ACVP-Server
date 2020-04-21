using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.DSA.Capabilities;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.DSA
{
	public class DSAKeyPair : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<KeyPairCapability> Capabilities { get; private set; } = new List<KeyPairCapability>();

		public DSAKeyPair(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "DSA", "keyGen")
		{
			//Prereqs
			//PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_1")));
			//PreReqs.Add(BuildPrereq("SHS", options.GetValue("DSA2_Prerequisite_SHA_2")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("DSA2_Prerequisite_DRBG")));

			if (options.GetValue("KeyPair_L2048N224") == "True") Capabilities.Add(new KeyPairCapability(2048, 224));
			if (options.GetValue("KeyPair_L2048N256") == "True") Capabilities.Add(new KeyPairCapability(2048, 256));
			if (options.GetValue("KeyPair_L3072N256") == "True") Capabilities.Add(new KeyPairCapability(3072, 256));
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.DSAKeyGen186_4
		{
			Capabilities = Capabilities.Select(x => new DSAKeyGen186_4.Capability
			{
				L = x.L,
				N = x.N
			}).ToList()
		};
	}
}
