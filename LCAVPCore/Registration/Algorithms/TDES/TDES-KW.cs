using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.TDES
{
	public class TDES_KW : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "kwCipher")]
		public List<string> KwCipher { get; set; } = new List<string>();

		[JsonProperty(PropertyName = "payloadLen")]
		public Domain PayloadLen { get; set; } = new Domain();

		public TDES_KW(Dictionary<string, string> options) : base("ACVP-TDES-KW")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("AES_Prerequisite")));
			PreReqs.Add(BuildPrereq("TDES", options.GetValue("TDES_Prerequisite")));

			if (options.GetValue("TKW_AE") == "True") Direction.Add("encrypt");
			if (options.GetValue("TKW_AD") == "True") Direction.Add("decrypt");

			if (options.GetValue("TKW_FWD_CIPHER") == "True") KwCipher.Add("cipher");
			if (options.GetValue("TKW_INV_CIPHER") == "True") KwCipher.Add("inverse");

			PayloadLen.Add(int.Parse(options.GetValue("TKW_PTLEN0")));
			PayloadLen.Add(int.Parse(options.GetValue("TKW_PTLEN1")));
			PayloadLen.Add(int.Parse(options.GetValue("TKW_PTLEN2")));
			PayloadLen.Add(int.Parse(options.GetValue("TKW_PTLEN3")));
			PayloadLen.Add(int.Parse(options.GetValue("TKW_PTLEN4")));
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.TDES_KW
		{
			Direction = Direction,
			Cipher = KwCipher,
			PayloadLength = PayloadLen.ToCoreDomain()
		};
	}
}
