using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.AES
{
	public class AES_CFB1 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; }

		public AES_CFB1(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-CFB1")
		{
			List<string> validValues = new List<string> { "Both", "Encrypt", "Decrypt" };
			KeyLen = new List<int>();
			if (validValues.Contains(options.GetValue("CFB1_128_State"))) KeyLen.Add(128);
			if (validValues.Contains(options.GetValue("CFB1_192_State"))) KeyLen.Add(192);
			if (validValues.Contains(options.GetValue("CFB1_256_State"))) KeyLen.Add(256);

			//Even though the direction is specified for each key length, it can't differ, so don't care how many have the direction value as long as one does
			Direction = new List<string>();
			if (options.ContainsValue("Encrypt") || options.ContainsValue("Both")) Direction.Add("encrypt");
			if (options.ContainsValue("Decrypt") || options.ContainsValue("Both")) Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.AES_CFB1
		{
			Direction = Direction,
			KeyLength = KeyLen
		};
	}
}
