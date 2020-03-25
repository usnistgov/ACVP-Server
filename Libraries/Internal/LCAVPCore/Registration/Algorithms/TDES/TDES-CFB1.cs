using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_CFB1 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyingOption")]
		public List<int> KeyingOption { get; private set; }

		public TDES_CFB1(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-TDES-CFB1")
		{
			KeyingOption = new List<int>();
			if (options.GetValue("CFB_KEY_CHOICE1") == "Yes") KeyingOption.Add(1);
			if (options.GetValue("CFB_KEY_CHOICE2") == "Yes") KeyingOption.Add(2);

			Direction = new List<string>();
			if (options.GetValue("CFB1_State") == "Encrypt" || options.GetValue("CFB1_State") == "Both") Direction.Add("encrypt");
			if (options.GetValue("CFB1_State") == "Decrypt" || options.GetValue("CFB1_State") == "Both") Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.TDES_CFB1
		{
			Direction = Direction,
			KeyingOption = KeyingOption
		};
	}
}
