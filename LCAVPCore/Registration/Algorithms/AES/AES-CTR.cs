using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_CTR : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get => new List<string> { "encrypt" }; }    //CAVS doesn't allow decrypt...

		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; } = new List<int>();

		//[JsonProperty(PropertyName = "ctrSource")]
		//public string CtrSource { get; private set; }

		public AES_CTR(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-CTR")
		{
			if (options.GetValue("CTR128_State") == "Encrypt") KeyLen.Add(128);
			if (options.GetValue("CTR192_State") == "Encrypt") KeyLen.Add(192);
			if (options.GetValue("CTR256_State") == "Encrypt") KeyLen.Add(256);

			//CtrSource = options.GetValue("CTR_Src") == "External" ? "external" : "internal";
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.AES_CTR
		{
			Direction = Direction,
			KeyLength = KeyLen
		};
	}
}
