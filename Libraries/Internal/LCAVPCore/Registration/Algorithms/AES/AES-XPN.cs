using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.AES
{
	public class AES_XPN : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "direction")]
		public List<string> Direction { get; private set; } = new List<string>();

		[JsonProperty(PropertyName = "keyLen")]
		public List<int> KeyLen { get; private set; } = new List<int>();

		[JsonProperty(PropertyName = "payloadLen")]
		public Domain PayloadLen { get; private set; } = new Domain();

		//public List<int> IVLen { get; set; } = new List<int>();

		[JsonProperty(PropertyName = "ivGen")]
		public string IVGen { get; set; }

		[JsonProperty(PropertyName = "ivGenMode", NullValueHandling = NullValueHandling.Ignore)]
		public string IVGenMode { get; set; }

		[JsonProperty(PropertyName = "saltGen")]
		public string SaltGen { get; set; }

		[JsonProperty(PropertyName = "aadLen")]
		public List<int> aadLen { get; set; } = new List<int>();

		[JsonProperty(PropertyName = "tagLen")]
		public List<int> TagLen { get; set; } = new List<int>();

		public AES_XPN(string ivGenLocation, string ivGenMode, Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ACVP-AES-XPN")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("AES_GCM_Prerequisite_AES")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("AES_GCM_Prerequisite_DRBG")));

			int value = 0;  //Used a number of times later on

			if (options.GetValue("Keysize_128") == "True") KeyLen.Add(128);
			if (options.GetValue("Keysize_192") == "True") KeyLen.Add(192);
			if (options.GetValue("Keysize_256") == "True") KeyLen.Add(256);

			//GCM/XPN does the direction differently than all the other AES flavors - this is a simpler way to do it

			if (options.GetValue("Encrypt") == "True") Direction.Add("encrypt");
			if (options.GetValue("Decrypt") == "True") Direction.Add("decrypt");

			if (options.GetValue("PTlength0") == "True") PayloadLen.Add(0);
			//In case they left one of the others 0, don't add it. Also make sure value not a duplicate.
			if (int.TryParse(options.GetValue("MinPTLength"), out value) && value != 0 && !PayloadLen.Contains(value)) PayloadLen.Add(value);
			if (int.TryParse(options.GetValue("MaxPTLength"), out value) && value != 0 && !PayloadLen.Contains(value)) PayloadLen.Add(value);
			if (int.TryParse(options.GetValue("MinPTLengthNonMult"), out value) && value != 0 && !PayloadLen.Contains(value)) PayloadLen.Add(value);
			if (int.TryParse(options.GetValue("MaxPTLengthNonMult"), out value) && value != 0 && !PayloadLen.Contains(value)) PayloadLen.Add(value);

			//if (options.GetValue("96bitIV") == "True") IVLen.Add(96);
			//if (options.GetValue("OtherIVlen") == "True")
			//{
			//	if (int.TryParse(options.GetValue("MinOtherIVlen"), out value) && !ivLen.Contains(value)) ivLen.Add(value);
			//	if (int.TryParse(options.GetValue("MaxOtherIVlen"), out value) && !ivLen.Contains(value)) ivLen.Add(value);
			//}

			IVGen = ivGenLocation; //options.GetValue("InternalIV") == "True" ? "internal" : "external";

			IVGenMode = ivGenMode;

			SaltGen = options.GetValue("XPNInternalSalt") == "True" ? "internal" : "external";

			if (options.GetValue("AADlength0") == "True") aadLen.Add(0);
			//In case they left one of the others 0, don't add it. Also make sure value not a duplicate.
			if (int.TryParse(options.GetValue("MinAADLength"), out value) && value != 0 && !aadLen.Contains(value)) aadLen.Add(value);
			if (int.TryParse(options.GetValue("MaxAADLength"), out value) && value != 0 && !aadLen.Contains(value)) aadLen.Add(value);
			if (int.TryParse(options.GetValue("MinAADLengthNonMult"), out value) && value != 0 && !aadLen.Contains(value)) aadLen.Add(value);
			if (int.TryParse(options.GetValue("MaxAADLengthNonMult"), out value) && value != 0 && !aadLen.Contains(value)) aadLen.Add(value);


			if (options.GetValue("Taglength_32") == "True") TagLen.Add(32);
			if (options.GetValue("Taglength_64") == "True") TagLen.Add(64);
			if (options.GetValue("Taglength_96") == "True") TagLen.Add(96);
			if (options.GetValue("Taglength_104") == "True") TagLen.Add(104);
			if (options.GetValue("Taglength_112") == "True") TagLen.Add(112);
			if (options.GetValue("Taglength_120") == "True") TagLen.Add(120);
			if (options.GetValue("Taglength_128") == "True") TagLen.Add(128);
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.AES_XPN
		{
			Direction = Direction,
			KeyLength = KeyLen,
			PayloadLength = PayloadLen.ToCoreDomain(),
			IVGen = IVGen,
			IVGenMode = IVGenMode,
			SaltGeneration = SaltGen,
			AADLength = ExtensionMethods.ToCoreDomain(aadLen),
			TagLength = TagLen
		};
	}
}
