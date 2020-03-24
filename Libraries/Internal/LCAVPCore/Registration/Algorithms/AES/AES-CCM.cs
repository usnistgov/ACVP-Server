using System;
using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;
using Range = LCAVPCore.Registration.MathDomain.Range;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_CCM : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; } = new List<int>();

		[JsonProperty(PropertyName = "payloadLen")]
		public Domain PtLen { get; set; } = new Domain();

		[JsonProperty(PropertyName = "ivLen")]
		public Domain IvLen { get; set; } = new Domain();

		[JsonProperty(PropertyName = "aadLen")]
		public Domain AadLen { get; set; } = new Domain();

		[JsonProperty(PropertyName = "tagLen")]
		public List<int> TagLen { get; set; } = new List<int>();


		public AES_CCM(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-CCM")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("AES_CCM_Prerequisite_AES")));

			//KeyLen
			if (options.GetValue("CCM_KeySize128") == "True") KeyLen.Add(128);
			if (options.GetValue("CCM_KeySize192") == "True") KeyLen.Add(192);
			if (options.GetValue("CCM_KeySize256") == "True") KeyLen.Add(256);

			//IvLen - Nonce in CAVS
			for (int i = 7; i <= 13; i++)
			{
				if (options.GetValue($"CCM_Nonce{i}") == "True") IvLen.Add(i * 8);
			}

			//TagLen
			for (int i = 2; i <= 8; i++)
			{
				if (options.GetValue($"CCM_Tag{i*2}") == "True") TagLen.Add(i * 16);	// x8 for bytes to bits, x2 because really want values 4-16
			}

			//PtLen
			int ptLenMin = ParsingHelper.ParseValueToInteger(options.GetValue("CCM_PlenMin")) * 8;
			int ptLenMax = ParsingHelper.ParseValueToInteger(options.GetValue("CCM_PlenMax")) * 8;

			if (ptLenMin == ptLenMax)
			{
				PtLen.Add(ptLenMin);
			}
			else
			{
				PtLen.Add(new Range()
				{
					Min = ptLenMin,
					Max = ptLenMax
				});
			}


			//AadLen
			int aadLenMin = ParsingHelper.ParseValueToInteger(options.GetValue("CCM_AdataMin")) * 8;
			int aadLenMax = Math.Max(ParsingHelper.ParseValueToInteger(options.GetValue("CCM_AdataMax")) * 8, options.GetValue("CCM_Adata216") == "True" ? 524288 : 0);      //Max of the max value provided or 2^16 * 8, if they checked that box

			if (aadLenMin == aadLenMax)
			{
				AadLen.Add(aadLenMin);
			}
			else {
				AadLen.Add(new Range()
				{
					Min = aadLenMin,
					Max = aadLenMax
				});
			}
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.AES_CCM
		{
			KeyLength = KeyLen,
			PayloadLength = PtLen.ToCoreDomain(),
			IVLength = IvLen.ToCoreDomain(),
			AADLength = AadLen.ToCoreDomain(),
			TagLength = TagLen
		};
	}
}
