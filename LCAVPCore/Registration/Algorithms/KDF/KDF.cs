using ACVPCore.Algorithms.Persisted;
using LCAVPCore.Registration.MathDomain;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LCAVPCore.Registration.Algorithms.KDF
{
	public class KDF : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("capabilities")]
		public List<Capability> Capabilities { get; private set; } = new List<Capability>();

		public KDF(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "KDF")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("KAS", options.GetValue("KDF108_Prerequisite_KAS")));
			PreReqs.Add(BuildPrereq("DRBG", options.GetValue("KDF108_Prerequisite_DRBG")));
			PreReqs.Add(BuildPrereq("AES", options.GetValue("KDF108_Prerequisite_CMAC")));
			PreReqs.Add(BuildPrereq("HMAC", options.GetValue("KDF108_Prerequisite_HMAC")));

			//Counter
			if (options.GetValue("KDF108_CTRMode") == "True")
			{
				Capability cap = new Capability { Mode = "counter" };

				//MAC Modes
				if (options.GetValue("KDF108_CTRModePRFCMACAES128") == "True") cap.MACModes.Add("CMAC-AES128");
				if (options.GetValue("KDF108_CTRModePRFCMACAES192") == "True") cap.MACModes.Add("CMAC-AES192");
				if (options.GetValue("KDF108_CTRModePRFCMACAES256") == "True") cap.MACModes.Add("CMAC-AES256");
				if (options.GetValue("KDF108_CTRModePRFCMACTDES3") == "True") cap.MACModes.Add("CMAC-TDES");
				if (options.GetValue("KDF108_CTRModePRFHMACSHA1") == "True") cap.MACModes.Add("HMAC-SHA-1");
				if (options.GetValue("KDF108_CTRModePRFHMACSHA224") == "True") cap.MACModes.Add("HMAC-SHA2-224");
				if (options.GetValue("KDF108_CTRModePRFHMACSHA256") == "True") cap.MACModes.Add("HMAC-SHA2-256");
				if (options.GetValue("KDF108_CTRModePRFHMACSHA384") == "True") cap.MACModes.Add("HMAC-SHA2-384");
				if (options.GetValue("KDF108_CTRModePRFHMACSHA512") == "True") cap.MACModes.Add("HMAC-SHA2-512");

				//Order
				if (options.GetValue("KDF108_CTRModePRFCounterBefore") == "True") cap.FixedDataOrder.Add("before fixed data");
				if (options.GetValue("KDF108_CTRModePRFCounterAfter") == "True") cap.FixedDataOrder.Add("after fixed data");
				if (options.GetValue("KDF108_CTRModePRFCounterMiddle") == "True") cap.FixedDataOrder.Add("middle fixed data");

				//Counter length - known as R in CAVS
				if (options.GetValue("KDF108_CTRModeRlen8") == "True") cap.CounterLength.Add(8);
				if (options.GetValue("KDF108_CTRModeRlen16") == "True") cap.CounterLength.Add(16);
				if (options.GetValue("KDF108_CTRModeRlen24") == "True") cap.CounterLength.Add(24);
				if (options.GetValue("KDF108_CTRModeRlen32") == "True") cap.CounterLength.Add(32);

				//Supported length - known as Ll in CAVS
				List<int> supportedLengths = new List<int>();
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_CTRModeLlenFULLMin")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_CTRModeLlenFULLMax")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_CTRModeLlenPARTMin")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_CTRModeLlenPARTMax")));

				cap.SupportedLengths.AddRange(supportedLengths.Where(x => x >= 1).Distinct().Cast<object>());

				Capabilities.Add(cap);
			}

			//Double Pipeline
			if (options.GetValue("KDF108_PipelineMode") == "True")
			{
				Capability cap = new Capability { Mode = "double pipeline iteration" };

				//MAC Modes
				if (options.GetValue("KDF108_PIPEModePRFCMACAES128") == "True") cap.MACModes.Add("CMAC-AES128");
				if (options.GetValue("KDF108_PIPEModePRFCMACAES192") == "True") cap.MACModes.Add("CMAC-AES192");
				if (options.GetValue("KDF108_PIPEModePRFCMACAES256") == "True") cap.MACModes.Add("CMAC-AES256");
				if (options.GetValue("KDF108_PIPEModePRFCMACTDES3") == "True") cap.MACModes.Add("CMAC-TDES");
				if (options.GetValue("KDF108_PIPEModePRFHMACSHA1") == "True") cap.MACModes.Add("HMAC-SHA-1");
				if (options.GetValue("KDF108_PIPEModePRFHMACSHA224") == "True") cap.MACModes.Add("HMAC-SHA2-224");
				if (options.GetValue("KDF108_PIPEModePRFHMACSHA256") == "True") cap.MACModes.Add("HMAC-SHA2-256");
				if (options.GetValue("KDF108_PIPEModePRFHMACSHA384") == "True") cap.MACModes.Add("HMAC-SHA2-384");
				if (options.GetValue("KDF108_PIPEModePRFHMACSHA512") == "True") cap.MACModes.Add("HMAC-SHA2-512");

				//Order - really should be optional, based on whether counter is used, but instead have to provide a dummy value
				if (options.GetValue("KDF108_PIPEModeCtrUsed") == "True")
				{
					if (options.GetValue("KDF108_PIPEModeCtrB4AarrayVar") == "True") cap.FixedDataOrder.Add("before iterator");
					if (options.GetValue("KDF108_PIPEModeCtrAftAarrayVar") == "True") cap.FixedDataOrder.Add("before fixed data");
					if (options.GetValue("KDF108_PIPEModeCtrAftFixedVar") == "True") cap.FixedDataOrder.Add("after fixed data");
				}
				else
				{
					cap.FixedDataOrder.Add("none");
				}

				//Counter length - known as R in CAVS - also should be optional, but have to provide 0 value if counter not used
				if (options.GetValue("KDF108_PIPEModeCtrUsed") == "True")
				{
					if (options.GetValue("KDF108_PIPEModeRlen8") == "True") cap.CounterLength.Add(8);
					if (options.GetValue("KDF108_PIPEModeRlen16") == "True") cap.CounterLength.Add(16);
					if (options.GetValue("KDF108_PIPEModeRlen24") == "True") cap.CounterLength.Add(24);
					if (options.GetValue("KDF108_PIPEModeRlen32") == "True") cap.CounterLength.Add(32);
				}
				else
				{
					cap.CounterLength.Add(0);
				}

				//Supported length - known as Ll in CAVS
				List<int> supportedLengths = new List<int>();
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_PIPEModeLlenFULLMin")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_PIPEModeLlenFULLMax")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_PIPEModeLlenPARTMin")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_PIPEModeLlenPARTMax")));

				cap.SupportedLengths.AddRange(supportedLengths.Where(x => x >= 1).Distinct().Cast<object>());

				Capabilities.Add(cap);
			}

			//Feedback
			if (options.GetValue("KDF108_FeedbackMode") == "True")
			{
				Capability cap = new Capability { Mode = "feedback" };

				//MAC Modes
				if (options.GetValue("KDF108_FDBKModePRFCMACAES128") == "True") cap.MACModes.Add("CMAC-AES128");
				if (options.GetValue("KDF108_FDBKModePRFCMACAES192") == "True") cap.MACModes.Add("CMAC-AES192");
				if (options.GetValue("KDF108_FDBKModePRFCMACAES256") == "True") cap.MACModes.Add("CMAC-AES256");
				if (options.GetValue("KDF108_FDBKModePRFCMACTDES3") == "True") cap.MACModes.Add("CMAC-TDES");
				if (options.GetValue("KDF108_FDBKModePRFHMACSHA1") == "True") cap.MACModes.Add("HMAC-SHA-1");
				if (options.GetValue("KDF108_FDBKModePRFHMACSHA224") == "True") cap.MACModes.Add("HMAC-SHA2-224");
				if (options.GetValue("KDF108_FDBKModePRFHMACSHA256") == "True") cap.MACModes.Add("HMAC-SHA2-256");
				if (options.GetValue("KDF108_FDBKModePRFHMACSHA384") == "True") cap.MACModes.Add("HMAC-SHA2-384");
				if (options.GetValue("KDF108_FDBKModePRFHMACSHA512") == "True") cap.MACModes.Add("HMAC-SHA2-512");

				//Order - really should be optional, based on whether counter is used, but instead have to provide a dummy value
				if (options.GetValue("KDF108_FDBKModeCtrInIterVar") == "True")
				{
					if (options.GetValue("KDF108_FDBKModeCtrB4IterVar") == "True") cap.FixedDataOrder.Add("before iterator");
					if (options.GetValue("KDF108_FDBKModeCtrAftIterVar") == "True") cap.FixedDataOrder.Add("before fixed data");
					if (options.GetValue("KDF108_FDBKModeCtrAftFixedVar") == "True") cap.FixedDataOrder.Add("after fixed data");
				}
				else
				{
					cap.FixedDataOrder.Add("none");
				}

				//Counter length - known as R in CAVS - also should be optional, but have to provide 0 value if counter not used
				if (options.GetValue("KDF108_FDBKModeCtrInIterVar") == "True")
				{
					if (options.GetValue("KDF108_FDBKModeRlen8") == "True") cap.CounterLength.Add(8);
					if (options.GetValue("KDF108_FDBKModeRlen16") == "True") cap.CounterLength.Add(16);
					if (options.GetValue("KDF108_FDBKModeRlen24") == "True") cap.CounterLength.Add(24);
					if (options.GetValue("KDF108_FDBKModeRlen32") == "True") cap.CounterLength.Add(32);
				}
				else
				{
					cap.CounterLength.Add(0);
				}

				//Supported length - known as Ll in CAVS
				List<int> supportedLengths = new List<int>();
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_FDBKModeLlenFULLMin")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_FDBKModeLlenFULLMax")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_FDBKModeLlenPARTMin")));
				supportedLengths.Add(8 * ParsingHelper.ParseValueToInteger(options.GetValue("KDF108_FDBKModeLlenPARTMax")));

				cap.SupportedLengths.AddRange(supportedLengths.Where(x => x >= 1).Distinct().Cast<object>());

				//Zero Length IV support
				cap.SupportsEmptyIV = options.GetValue("KDF108_FDBKModeZeroLenIVNOTSupported") != "True";

				Capabilities.Add(cap);
			}

		}

		public class Capability
		{
			[JsonProperty("kdfMode")]
			public string Mode { get; set; }

			[JsonProperty("macMode")]
			public List<string> MACModes { get; set; } = new List<string>();

			[JsonProperty("supportedLengths")]
			public Domain SupportedLengths { get; set; } = new Domain();

			[JsonProperty("fixedDataOrder")]
			public List<string> FixedDataOrder { get; set; } = new List<string>();

			[JsonProperty("counterLength")]
			public List<int> CounterLength { get; set; } = new List<int>();

			[JsonProperty("supportsEmptyIv", NullValueHandling = NullValueHandling.Ignore)]
			public bool? SupportsEmptyIV { get; set; }
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.KDF
		{
			Capabilities = Capabilities.Select(x => new ACVPCore.Algorithms.Persisted.KDF.Capability
			{
				KDFMode = x.Mode,
				MacMode = x.MACModes,
				SupportedLengths = x.SupportedLengths.ToCoreDomain(),
				FixedDataOrder = x.FixedDataOrder,
				CounterLength = x.CounterLength,
				SupportsEmptyIV = x.SupportsEmptyIV
			}).ToList()
		};
	}
}
