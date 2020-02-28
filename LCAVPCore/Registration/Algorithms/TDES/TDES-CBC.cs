using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_CBC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyingOption")]
		public List<int> KeyingOption { get; private set; }

		public TDES_CBC(Dictionary<string, string> options) : base("ACVP-TDES-CBC")
		{
			KeyingOption = new List<int>();
			if (options.GetValue("CBC_KEY_CHOICE1") == "Yes") KeyingOption.Add(1);
			if (options.GetValue("CBC_KEY_CHOICE2") == "Yes") KeyingOption.Add(2);

			Direction = new List<string>();
			if (options.GetValue("CBC_State") == "Encrypt" || options.GetValue("CBC_State") == "Both") Direction.Add("encrypt");
			if (options.GetValue("CBC_State") == "Decrypt" || options.GetValue("CBC_State") == "Both") Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.TDES_CBC
		{
			Direction = Direction,
			KeyingOption = KeyingOption
		};
	}
}
