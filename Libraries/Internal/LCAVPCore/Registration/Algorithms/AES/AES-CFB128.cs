using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_CFB128 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; }

		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; }

		public AES_CFB128(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-CFB128")
		{
			List<string> validValues = new List<string> { "Both", "Encrypt", "Decrypt" };
			KeyLen = new List<int>();
			if (validValues.Contains(options.GetValue("CFB128_128_State"))) KeyLen.Add(128);
			if (validValues.Contains(options.GetValue("CFB128_192_State"))) KeyLen.Add(192);
			if (validValues.Contains(options.GetValue("CFB128_256_State"))) KeyLen.Add(256);

			//Even though the direction is specified for each key length, it can't differ, so don't care how many have the direction value as long as one does
			Direction = new List<string>();
			if (options.ContainsValue("Encrypt") || options.ContainsValue("Both")) Direction.Add("encrypt");
			if (options.ContainsValue("Decrypt") || options.ContainsValue("Both")) Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.AES_CFB128
		{
			Direction = Direction,
			KeyLength = KeyLen
		};
	}
}
