using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_OFB : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; } = new List<int>();

		public AES_OFB(Dictionary<string, string> options) : base("ACVP-AES-OFB")
		{
			List<string> validValues = new List<string> { "Both", "Encrypt", "Decrypt" };
			if (validValues.Contains(options.GetValue("OFB128_State"))) KeyLen.Add(128);
			if (validValues.Contains(options.GetValue("OFB192_State"))) KeyLen.Add(192);
			if (validValues.Contains(options.GetValue("OFB256_State"))) KeyLen.Add(256);

			//Even though the direction is specified for each key length, it can't differ, so don't care how many have the direction value as long as one does
			if (options.ContainsValue("Encrypt") || options.ContainsValue("Both")) Direction.Add("encrypt");
			if (options.ContainsValue("Decrypt") || options.ContainsValue("Both")) Direction.Add("decrypt");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.AES_OFB
		{
			Direction = Direction,
			KeyLength = KeyLen
		};
	}
}
