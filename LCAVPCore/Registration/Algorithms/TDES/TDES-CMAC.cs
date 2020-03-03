using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_CMAC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();		

		public TDES_CMAC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "CMAC-TDES")
		{
			if (options.GetValue("CMACGen_TDES3") == "True")
			{
				Capabilities.Add(BuildCapability("gen", 3, options));  //The 3 means 3 distinct keys, which is keying option 1. Sigh.
			}

			if (options.GetValue("CMACVer_TDES2") == "True")
			{
				Capabilities.Add(BuildCapability("ver", 2, options));
			}

			if (options.GetValue("CMACVer_TDES3") == "True")
			{
				Capabilities.Add(BuildCapability("ver", 3, options));  //The 3 means 3 distinct keys, which is keying option 1. Sigh.
			}
		}

		private Capability BuildCapability(string direction, int numberOfKeys, Dictionary<string, string> options)
		{
			Capability capability = new Capability();

			int value;

			//Explicitly pass in whether this is gen or ver, since there are cascading differences
			capability.Direction.Add(direction);

			if (direction == "gen")
			{
				//Know that is keying option 1 (3 keys), ko 2 is no longer valid
				capability.KeyingOption.Add(KeyingOption(numberOfKeys));

				//Deal with all the message length values. Make sure values are positive (where not 0) and not already in the collection
				if (options.GetValue("CMACGen_TDES3_(K_EQ_0)") == "True") capability.MsgLen.Add(0);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_(K_mod_B_EQ_0)_1"));
				if (value > 0) capability.MsgLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_(K_mod_B_EQ_0)_2"));
				if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_(K_mod_B<>0)_1"));
				if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_(K_mod_B<>0)_2"));
				if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

				//Only add the max value if 2^16 is not checked
				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_KMAX"));
				if (value > 0 && !capability.MsgLen.Contains(value) && options.GetValue("CMACGen_TDES3_(KMAX_EQ_2^16)") != "True") capability.MsgLen.Add(value);

				if (options.GetValue("CMACGen_TDES3_(KMAX_EQ_2^16)") == "True" && !capability.MsgLen.Contains(524288)) capability.MsgLen.Add(524288);


				//MacLen
				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_TlenMin"));
				if (value > 0) capability.MacLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_TlenMid"));
				if (value > 0 && !capability.MacLen.Contains(value)) capability.MacLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue("CMACGen_TDES3_TlenMax"));
				if (value > 0 && !capability.MacLen.Contains(value)) capability.MacLen.Add(value);
			}
			else
			{
				capability.KeyingOption.Add(KeyingOption(numberOfKeys));

				//Deal with all the message length values. Make sure values are positive (where not 0) and not already in the collection
				if (options.GetValue($"CMACVer_TDES{numberOfKeys}_(K_EQ_0)") == "True") capability.MsgLen.Add(0);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_(K_mod_B_EQ_0)_1"));
				if (value > 0) capability.MsgLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_(K_mod_B_EQ_0)_2"));
				if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_(K_mod_B<>0)_1"));
				if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_(K_mod_B<>0)_2"));
				if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

				//Only add the max value if 2^16 is not checked
				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_KMAX"));
				if (value > 0 && !capability.MsgLen.Contains(value) && options.GetValue($"CMACVer_TDES{numberOfKeys}_(KMAX_EQ_2^16)") != "True") capability.MsgLen.Add(value);

				if (options.GetValue($"CMACVer_TDES{numberOfKeys}_(KMAX_EQ_2^16)") == "True" && !capability.MsgLen.Contains(524288)) capability.MsgLen.Add(524288);


				//MacLen
				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_TlenMin"));
				if (value > 0) capability.MacLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_TlenMid"));
				if (value > 0 && !capability.MacLen.Contains(value)) capability.MacLen.Add(value);

				value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMACVer_TDES{numberOfKeys}_TlenMax"));
				if (value > 0 && !capability.MacLen.Contains(value)) capability.MacLen.Add(value);
			}

			return capability;
		}

		private int KeyingOption(int numberOfKeys)
		{
			int keyingOption;
			switch (numberOfKeys)
			{
				case 1: keyingOption = 3; break;
				case 2: keyingOption = 2; break;
				case 3: keyingOption = 1; break;
				default: keyingOption = 0; break;
			}
			return keyingOption;
		}

		public class Capability
		{
			[JsonProperty(PropertyName = "direction")]
			public List<string> Direction { get; private set; } = new List<string>();

			[JsonProperty(PropertyName = "keyingOption")]
			public List<int> KeyingOption { get; private set; } = new List<int>();

			[JsonProperty(PropertyName = "msgLen")]
			public Domain MsgLen { get; private set; } = new Domain();

			[JsonProperty(PropertyName = "macLen")]
			public Domain MacLen { get; private set; } = new Domain();
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.TDES_CMAC
		{
			Capabilities = Capabilities.Select(x => new ACVPCore.Algorithms.Persisted.TDES_CMAC.CapabilityObject
			{

				KeyingOption = x.KeyingOption,
				Direction = x.Direction,
				MacLength = x.MacLen.ToCoreDomain(),
				MessageLength = x.MsgLen.ToCoreDomain()
			}).ToList()
		};
	}
}
