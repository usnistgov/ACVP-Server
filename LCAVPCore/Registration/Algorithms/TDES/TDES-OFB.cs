using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_OFB : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "keyingOption")]
		public List<int> KeyingOption { get; private set; } = new List<int>();

		public TDES_OFB(Dictionary<string, string> options) : base("ACVP-TDES-OFB")
		{
			if (options.GetValue("OFB_KEY_CHOICE1") == "Yes") KeyingOption.Add(1);
			if (options.GetValue("OFB_KEY_CHOICE2") == "Yes") KeyingOption.Add(2);

			if (options.GetValue("OFB_State") == "Encrypt" || options.GetValue("OFB_State") == "Both") Direction.Add("encrypt");
			if (options.GetValue("OFB_State") == "Decrypt" || options.GetValue("OFB_State") == "Both") Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.TDES_OFB
		{
			Direction = Direction,
			KeyingOption = KeyingOption
		};
	}
}
