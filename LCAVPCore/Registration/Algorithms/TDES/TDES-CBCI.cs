using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_CBCI : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyingOption")]
		public List<int> KeyingOption { get; private set; }

		public TDES_CBCI(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-TDES-CBCI")
		{
			KeyingOption = new List<int>();
			if (options.GetValue("CBCI_KEY_CHOICE1") == "Yes") KeyingOption.Add(1);
			if (options.GetValue("CBCI_KEY_CHOICE2") == "Yes") KeyingOption.Add(2);

			Direction = new List<string>();
			if (options.GetValue("CBCI_State") == "Encrypt" || options.GetValue("CBCI_State") == "Both") Direction.Add("encrypt");
			if (options.GetValue("CBCI_State") == "Decrypt" || options.GetValue("CBCI_State") == "Both") Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.TDES_CBCI
		{
			Direction = Direction,
			KeyingOption = KeyingOption
		};
	}
}
