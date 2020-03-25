using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_OFBI : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyingOption")]
		public List<int> KeyingOption { get; private set; }

		public TDES_OFBI(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-TDES-OFBI")
		{
			KeyingOption = new List<int>();
			if (options.GetValue("OFBI_KEY_CHOICE1") == "Yes") KeyingOption.Add(1);
			if (options.GetValue("OFBI_KEY_CHOICE2") == "Yes") KeyingOption.Add(2);

			Direction = new List<string>();
			if (options.GetValue("OFBI_State") == "Encrypt" || options.GetValue("OFBI_State") == "Both") Direction.Add("encrypt");
			if (options.GetValue("OFBI_State") == "Decrypt" || options.GetValue("OFBI_State") == "Both") Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.TDES_OFBI
		{
			Direction = Direction,
			KeyingOption = KeyingOption
		};
	}
}
