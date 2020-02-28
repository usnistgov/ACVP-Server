using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class SRTP : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "aesKeyLength")]
		public List<int> AESKeyLengths { get; private set; } = new List<int>();

		[JsonProperty(PropertyName = "kdrExponent")]
		public List<int> KDRExponents { get; private set; } = new List<int>();

		[JsonProperty(PropertyName = "supportsZeroKdr")]
		public bool SupportsZeroKDR { get; private set; }

		public SRTP(Dictionary<string, string> options) : base("kdf-components", "srtp")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("KDF_800_135_Prerequisite_AES")));

			//AES Key Lengths
			if (options.GetValue("KDF_800_135_SRTP_AES_128") == "True") AESKeyLengths.Add(128);
			if (options.GetValue("KDF_800_135_SRTP_AES_192") == "True") AESKeyLengths.Add(192);
			if (options.GetValue("KDF_800_135_SRTP_AES_256") == "True") AESKeyLengths.Add(256);

			//KDR Exponents
			//Loop through all options, 2^1 through 2^24, and add the selected ones - including if all is checked)
			for (int i = 1; i < 25; i++)
			{
				if (options.GetValue("KDF_800_135_SRTP_KDROption") == "All" || options.GetValue($"KDF_800_135_SRTP_KDR_2^{i}") == "True") KDRExponents.Add(i);
			}

			//Supports Zero KDR
			SupportsZeroKDR = options.GetValue("KDF_800_135_SRTP_KDR_0") == "True";
		}

		public bool ShouldSerializeKDRExponents()
		{
			return KDRExponents.Count > 0;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KDF_SRTP
		{
			AESKeyLengths = AESKeyLengths,
			KDRExponents = KDRExponents,
			SupportsZeroKDR = SupportsZeroKDR
		};
	}
}
