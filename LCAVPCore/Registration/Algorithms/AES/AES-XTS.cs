using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_XTS : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; set; }

		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "payloadLen")]
		public Domain PayloadLen { get; set; } = new Domain();

		[JsonProperty(PropertyName = "tweakMode")]
		public List<string> TweakMode { get; private set; } = new List<string>();

		public AES_XTS(int keyLength, Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-XTS")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("XTS-AES_Prerequisite_AES")));

			//Most of the options are specific to the key length, and the JSON format requires separate registrations if the options differ, so play it safe and just do them separate

			//KeyLen is pretty simple, since it is the basis for this whole registration
			KeyLen = new List<int> { keyLength };

			//Direction
			if (options.GetValue($"XTS_AES{keyLength}_Encrypt") == "True") Direction.Add("encrypt");
			if (options.GetValue($"XTS_AES{keyLength}_Decrypt") == "True") Direction.Add("decrypt");

			//PtLen
			//Take all the non-zero unique values of the 5 data unit (pt) length lines.
			Regex dataUnitLengthPattern = new Regex($"^XTS_AES{keyLength}_DataUnitLength[1-5]$");
			int intValue;
			foreach (string value in options.Where(o => dataUnitLengthPattern.IsMatch(o.Key)).Select(o => o.Value).Distinct())
			{
				intValue = ParsingHelper.ParseValueToInteger(value);

				if (intValue >= 128 && intValue <= 65536)
				{
					PayloadLen.Add(intValue);
				}
			}

			//Add 2^16 if checked and not already in list
			if (options.GetValue($"XTS_AES{keyLength}_DataUnitLength2^16") == "True" && !PayloadLen.Contains(65536)) PayloadLen.Add(65536);

			//Tweak format
			if (options.GetValue("Use128BitTweak") == "True") TweakMode.Add("hex");
			if (options.GetValue("UseDataUnitSequenceNumber") == "True") TweakMode.Add("number");
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.AES_XTS
		{
			Direction = Direction,
			KeyLength = KeyLen,
			PayloadLength = PayloadLen.ToCoreDomain(),
			TweakMode = TweakMode
		};
	}
}
