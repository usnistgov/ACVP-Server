using System.Collections.Generic;
using System.Linq;
using NIST.CVP.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_KW : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; } = new List<int>();

		[JsonProperty(PropertyName = "kwCipher")]
		public List<string> KwCipher { get; set; } = new List<string>();

		[JsonProperty(PropertyName = "payloadLen")]
		public Domain PayloadLen { get; set; } = new Domain();

		public AES_KW(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-KW")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("AES_Prerequisite")));
			PreReqs.Add(BuildPrereq("TDES", options.GetValue("TDES_Prerequisite")));

			if (options.GetValue("KW_AES_128") == "True") KeyLen.Add(128);
			if (options.GetValue("KW_AES_192") == "True") KeyLen.Add(192);
			if (options.GetValue("KW_AES_256") == "True") KeyLen.Add(256);

			if (options.GetValue("KW_AE") == "True") Direction.Add("encrypt");
			if (options.GetValue("KW_AD") == "True") Direction.Add("decrypt");

			if (options.GetValue("KW_FWD_CIPHER") == "True") KwCipher.Add("cipher");
			if (options.GetValue("KW_INV_CIPHER") == "True") KwCipher.Add("inverse");

			//Since there are a number of weird things they can do to wind up with duplicate values, first build a temp collection of the values, then put the distinct values in the final collection
			List<int> payloadLenValues = new List<int>();

			payloadLenValues.Add(ParsingHelper.ParseValueToInteger(options.GetValue("KW_PTLEN0")));
			payloadLenValues.Add(ParsingHelper.ParseValueToInteger(options.GetValue("KW_PTLEN1")));
			payloadLenValues.Add(ParsingHelper.ParseValueToInteger(options.GetValue("KW_PTLEN2")));
			payloadLenValues.Add(ParsingHelper.ParseValueToInteger(options.GetValue("KW_PTLEN3")));
			payloadLenValues.Add(ParsingHelper.ParseValueToInteger(options.GetValue("KW_PTLEN4")));

			PayloadLen.AddRange(payloadLenValues.Where(x => x >= 128).Distinct().Cast<object>());
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.AES_KW
		{
			Direction = Direction,
			KeyLength = KeyLen,
			Cipher = KwCipher,
			PayloadLength = PayloadLen.ToCoreDomain()
		};
	}
}
