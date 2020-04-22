using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.HMAC
{
	public class HMAC_SHA_1 : AlgorithmBase, IAlgorithm
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

		public HMAC_SHA_1(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "HMAC-SHA-1")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("HMAC_SHA_Prerequisite")));
			//PreReqs.Add(BuildPrereq("SHA-3", options.GetValue("HMAC_SHA3_Prerequisite")));

			////Since there are a number of weird things they can do to wind up with duplicate values, first build a temp collection of the values, then put the distinct values in the final collection
			//List<int> keyLenValues = new List<int>();

			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K<B_1")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K<B_2")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K>B_1")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K>B_2")));
			//if (options.GetValue("HMAC_SHA1_K_EQ_B") == "True") keyLenValues.Add(512);

			//KeyLen.AddRange(keyLenValues.Where(x => x >= 8).Distinct().Cast<object>());

			KeyLessThanBlockSize = ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K<B_1")) > 0 || ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K<B_2")) > 0;
			KeyGreaterThanBlockSize = ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K>B_1")) > 0 || ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA1_K>B_2")) > 0;
			KeyEqualBlockSize = options.GetValue("HMAC_SHA1_K_EQ_B") == "True";

			if (options.GetValue("HMAC_SHA1_MAC10") == "True") MacLen.Add(80);
			if (options.GetValue("HMAC_SHA1_MAC12") == "True") MacLen.Add(96);
			if (options.GetValue("HMAC_SHA1_MAC16") == "True") MacLen.Add(128);
			if (options.GetValue("HMAC_SHA1_MAC20") == "True") MacLen.Add(160);
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.HMAC_SHA_1
		{
			KeyLessThanBlockSize = KeyLessThanBlockSize,
			KeyEqualBlockSize = KeyEqualBlockSize,
			KeyGreaterThanBlockSize = KeyGreaterThanBlockSize,
			MacLength = MacLen.ToCoreDomain()
		};
	}
}
