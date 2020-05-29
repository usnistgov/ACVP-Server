using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.MathDomain;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.AES
{
	public class AES_CMAC : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public class Capability
		{
			[JsonProperty(PropertyName = "direction")]
			public List<string> Direction { get; set; } = new List<string>();

			[JsonProperty(PropertyName = "keyLen")]
			public List<int> KeyLen { get; set; } = new List<int>();

			[JsonProperty(PropertyName = "msgLen")]
			public Domain MsgLen { get; set; } = new Domain();

			[JsonProperty(PropertyName = "macLen")]
			public Domain MacLen { get; set; } = new Domain();
		}

		public AES_CMAC(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "CMAC-AES")
		{
			//Need to loop through the 6 combinations of direction and keyLen to create the capabilities. Could try to merge them, but no...
			foreach (string direction in new string[] { "Gen", "Ver" })
			{
				if (options.GetValue($"CMAC{direction}_AES") == "True")
				{
					foreach (int keyLen in new int[] { 128, 192, 256 })
					{
						if (options.GetValue($"CMAC{direction}_AES{keyLen}") == "True")
						{
							Capabilities.Add(BuildCapability(direction, keyLen, options));
						}
					}
				}
			}

		}

		private Capability BuildCapability(string direction, int keyLen, Dictionary<string, string> options)
		{

			Capability capability = new Capability();

			int value;

			capability.Direction.Add(direction.ToLower());

			capability.KeyLen.Add(keyLen);

			//Deal with all the message length values. Make sure values are positive (where not 0) and not already in the collection
			if (options.GetValue($"CMAC{direction}_AES{keyLen}_(K_EQ_0)") == "True") capability.MsgLen.Add(0);

			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_(K_mod_B_EQ_0)_1"));
			if (value > 0) capability.MsgLen.Add(value);

			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_(K_mod_B_EQ_0)_2"));
			if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_(K_mod_B<>0)_1"));
			if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_(K_mod_B<>0)_2"));
			if (value > 0 && !capability.MsgLen.Contains(value)) capability.MsgLen.Add(value);

			//Only add the max value if 2^16 is not checked
			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_KMAX"));
			if (value > 0 && !capability.MsgLen.Contains(value) && options.GetValue($"CMAC{direction}_AES{keyLen}_(KMAX_EQ_2^16)") != "True") capability.MsgLen.Add(value);

			if (options.GetValue($"CMAC{direction}_AES{keyLen}_(KMAX_EQ_2^16)") == "True" && !capability.MsgLen.Contains(524288)) capability.MsgLen.Add(524288);


			//MacLen
			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_TlenMin"));
			if (value > 0) capability.MacLen.Add(value);

			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_TlenMid"));
			if (value > 0 && !capability.MacLen.Contains(value)) capability.MacLen.Add(value);

			value = 8 * ParsingHelper.ParseValueToInteger(options.GetValue($"CMAC{direction}_AES{keyLen}_TlenMax"));
			if (value > 0 && !capability.MacLen.Contains(value)) capability.MacLen.Add(value);

			return capability;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.AES_CMAC
		{
			Capabilities = Capabilities.Select(x => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.AES_CMAC.Capability
			{

				KeyLength = x.KeyLen,
				Direction = x.Direction,
				MacLength = x.MacLen.ToCoreDomain(),
				MessageLength = x.MsgLen.ToCoreDomain()
			}).ToList()
		};
	}
}
