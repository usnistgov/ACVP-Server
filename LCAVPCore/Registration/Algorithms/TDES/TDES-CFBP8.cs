using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_CFBP8 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyingOption")]
		public List<int> KeyingOption { get; private set; }

		public TDES_CFBP8(Dictionary<string, string> options) : base("ACVP-TDES-CFBP8")
		{
			KeyingOption = new List<int>();
			if (options.GetValue("CFBP_KEY_CHOICE1") == "Yes") KeyingOption.Add(1);
			if (options.GetValue("CFBP_KEY_CHOICE2") == "Yes") KeyingOption.Add(2);

			Direction = new List<string>();
			if (options.GetValue("CFBP8_State") == "Encrypt" || options.GetValue("CFBP8_State") == "Both") Direction.Add("encrypt");
			if (options.GetValue("CFBP8_State") == "Decrypt" || options.GetValue("CFBP8_State") == "Both") Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.TDES_CFBP8
		{
			Direction = Direction,
			KeyingOption = KeyingOption
		};
	}
}
