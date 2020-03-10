using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.DRBG
{
	public class CTR : AlgorithmBase, IAlgorithm
	{
		private const int TDEA_BLOCK_SIZE = 64;
		private const int AES128_BLOCK_SIZE = 128;
		private const int AES192_BLOCK_SIZE = 128;
		private const int AES256_BLOCK_SIZE = 128;


		[JsonProperty("reseedImplemented")]
		public bool ReseedImplemented { get; private set; }

		[JsonProperty("predResistanceEnabled")]
		public List<bool> PredictionResistance { get; private set; } = new List<bool>();

		[JsonProperty("capabilities")]
		public List<ModeParameters> Modes { get; private set; } = new List<ModeParameters>();

		public CTR(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "ctrDRBG")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("AES", options.GetValue("CTRDRBG_usedf_Prerequisite_AES")));
			PreReqs.Add(BuildPrereq("AES", options.GetValue("CTRDRBG_nodf_Prerequisite_AES")));
			PreReqs.Add(BuildPrereq("TDES", options.GetValue("CTRDRBG_usedf_Prerequisite_TDES")));
			PreReqs.Add(BuildPrereq("TDES", options.GetValue("CTRDRBG_nodf_Prerequisite_TDES")));

			//Reseed
			ReseedImplemented = options.GetValue("CTR_DRBG NO RESEED") == "False";

			//Prediction resistance
			if (options.GetValue("CTR_DRBG PREDICTION RESISTANCE ENABLED") == "True") PredictionResistance.Add(true);
			if (options.GetValue("CTR_DRBG PREDICTION RESISTANCE NOT ENABLED") == "True") PredictionResistance.Add(false);

			//Grab the # of output blocks, will need this in each mode, have to do math to get the right value
			int blocks = ParsingHelper.ParseValueToInteger(options.GetValue("CTR_DRBG NumberOfOutputBlocks"));

			//Modes - need to do twice because Derivation Function can be specified twice
			if (options.GetValue("CTR_DRBG Use Df") == "True")
			{
				if (options.GetValue("CTR_DRBG 3KeyTDEA") == "True") Modes.Add(new ModeParameters("TDES", options.GetValue("CTR_DRBG 3KeyTDEA use df lengths"), blocks, TDEA_BLOCK_SIZE, true));
				if (options.GetValue("CTR_DRBG AES-128") == "True") Modes.Add(new ModeParameters("AES-128", options.GetValue("CTR_DRBG AES-128 use df lengths"), blocks, AES128_BLOCK_SIZE, true));
				if (options.GetValue("CTR_DRBG AES-192") == "True") Modes.Add(new ModeParameters("AES-192", options.GetValue("CTR_DRBG AES-192 use df lengths"), blocks, AES192_BLOCK_SIZE, true));
				if (options.GetValue("CTR_DRBG AES-256") == "True") Modes.Add(new ModeParameters("AES-256", options.GetValue("CTR_DRBG AES-256 use df lengths"), blocks, AES256_BLOCK_SIZE, true));
			}

			if (options.GetValue("CTR_DRBG No Df") == "True")
			{
				if (options.GetValue("CTR_DRBG 3KeyTDEA") == "True") Modes.Add(new ModeParameters("TDES", options.GetValue("CTR_DRBG 3KeyTDEA no df lengths"), blocks, TDEA_BLOCK_SIZE, false));
				if (options.GetValue("CTR_DRBG AES-128") == "True") Modes.Add(new ModeParameters("AES-128", options.GetValue("CTR_DRBG AES-128 no df lengths"), blocks, AES128_BLOCK_SIZE, false));
				if (options.GetValue("CTR_DRBG AES-192") == "True") Modes.Add(new ModeParameters("AES-192", options.GetValue("CTR_DRBG AES-192 no df lengths"), blocks, AES192_BLOCK_SIZE, false));
				if (options.GetValue("CTR_DRBG AES-256") == "True") Modes.Add(new ModeParameters("AES-256", options.GetValue("CTR_DRBG AES-256 no df lengths"), blocks, AES256_BLOCK_SIZE, false));
			}

		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.CounterDRBG
		{
			ReseedImplemented = ReseedImplemented,
			PredictionResistanceEnabled = PredictionResistance,
			Capabilities = Modes.Select(x => new CounterDRBG.Capability
			{
				Mode = x.Mode,
				DerivationFunctionEnabled = (bool)x.DerivationFunction,
				AdditionalInputLength = x.AdditionalInputLength.ToCoreDomain(),
				EntropyInputLength = x.EntropyInputLength.ToCoreDomain(),
				NonceLength = x.NonceLength.ToCoreDomain(),
				PersonalizationStringLength = x.PersonalizationStringLength.ToCoreDomain(),
				ReturnedBitsLength = x.ReturnedBitsLength
			}).ToList()
		};
	}
}
