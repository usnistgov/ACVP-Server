﻿using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.HMAC
{
	public class HMAC_SHA2_512 : AlgorithmBase, IAlgorithm
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

		public HMAC_SHA2_512(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "HMAC-SHA2-512")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("HMAC_SHA_Prerequisite")));
			//PreReqs.Add(BuildPrereq("SHA-3", options.GetValue("HMAC_SHA3_Prerequisite")));

			////Since there are a number of weird things they can do to wind up with duplicate values, first build a temp collection of the values, then put the distinct values in the final collection
			//List<int> keyLenValues = new List<int>();

			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K<B_1")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K<B_2")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K>B_1")));
			//keyLenValues.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K>B_2")));
			//if (options.GetValue("HMAC_SHA512_K_EQ_B") == "True") keyLenValues.Add(1024);

			//KeyLen.AddRange(keyLenValues.Where(x => x >= 8).Distinct().Cast<object>());

			KeyLessThanBlockSize = ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K<B_1")) > 0 || ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K<B_2")) > 0;
			KeyGreaterThanBlockSize = ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K>B_1")) > 0 || ParsingHelper.ParseValueToInteger(options.GetValue("HMAC_SHA512_K>B_2")) > 0;
			KeyEqualBlockSize = options.GetValue("HMAC_SHA512_K_EQ_B") == "True";

			if (options.GetValue("HMAC_SHA512_MAC32") == "True") MacLen.Add(32 * 8);
			if (options.GetValue("HMAC_SHA512_MAC40") == "True") MacLen.Add(40 * 8);
			if (options.GetValue("HMAC_SHA512_MAC48") == "True") MacLen.Add(48 * 8);
			if (options.GetValue("HMAC_SHA512_MAC56") == "True") MacLen.Add(56 * 8);
			if (options.GetValue("HMAC_SHA512_MAC64") == "True") MacLen.Add(64 * 8);
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.HMAC_SHA2_512
		{
			KeyLessThanBlockSize = KeyLessThanBlockSize,
			KeyEqualBlockSize = KeyEqualBlockSize,
			KeyGreaterThanBlockSize = KeyGreaterThanBlockSize,
			MacLength = MacLen.ToCoreDomain()
		};
	}
}