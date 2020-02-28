using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.HMAC
{
	public class HMAC_SHA3_256 : AlgorithmBase, IAlgorithm
	{
		//[JsonProperty(PropertyName = "keyLen")]
		//public Domain KeyLen { get; set; } = new Domain();

		[JsonProperty(PropertyName = "keyLTblock")]
		public bool KeyLessThanBlockSize { get; set; }

		[JsonProperty(PropertyName = "keyEQblock")]
		public bool KeyEqualBlockSize { get; set; }

		[JsonProperty(PropertyName = "keyGTblock")]
		public bool KeyGreaterThanBlockSize { get; set; }

		[JsonProperty(PropertyName = "macLen")]
		public Domain MacLen { get; set; } = new Domain();

		public HMAC_SHA3_256(Dictionary<string, string> options) : base("HMAC-SHA3-256")
		{
			//Prereqs
			//PreReqs.Add(BuildPrereq("SHS", options.GetValue("HMAC_SHA_Prerequisite")));
			PreReqs.Add(BuildPrereq("SHA-3", options.GetValue("HMAC_SHA3_Prerequisite")));

			////Since there are a number of weird things they can do to wind up with duplicate values, first build a temp collection of the values, then put the distinct values in the final collection
			//List<int> keyLenValues = new List<int>();

			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K<B_1")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K<B_2")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K>B_1")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K>B_2")));
			//if (options.GetValue("HMAC_SHA256_K_EQ_B") == "True") keyLenValues.Add(512);

			//KeyLen.AddRange(keyLenValues.Where(x => x >= 8).Distinct().Cast<object>());

			KeyLessThanBlockSize = ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K<B_1")) > 0 || ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K<B_2")) > 0;
			KeyGreaterThanBlockSize = ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K>B_1")) > 0 || ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA256_K>B_2")) > 0;
			KeyEqualBlockSize = options.GetValue("HMAC_SHA256_K_EQ_B") == "True";

			if (options.GetValue("HMAC_SHA256_MAC16") == "True") MacLen.Add(16 * 8);
			if (options.GetValue("HMAC_SHA256_MAC24") == "True") MacLen.Add(24 * 8);
			if (options.GetValue("HMAC_SHA256_MAC32") == "True") MacLen.Add(32 * 8);
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.HMAC_SHA3_256
		{
			KeyLessThanBlockSize = KeyLessThanBlockSize,
			KeyEqualBlockSize = KeyEqualBlockSize,
			KeyGreaterThanBlockSize = KeyGreaterThanBlockSize,
			MacLength = MacLen.ToCoreDomain()
		};
	}
}
