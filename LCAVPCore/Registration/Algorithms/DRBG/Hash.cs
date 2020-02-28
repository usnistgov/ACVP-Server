using System.Collections.Generic;
using System.Linq;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.DRBG
{
	public class Hash : AlgorithmBase, IAlgorithm
	{
		private const int SHA1_BLOCK_SIZE = 160;
		private const int SHA224_BLOCK_SIZE = 224;
		private const int SHA256_BLOCK_SIZE = 256;
		private const int SHA384_BLOCK_SIZE = 384;
		private const int SHA512_BLOCK_SIZE = 512;
		private const int SHA512224_BLOCK_SIZE = 224;
		private const int SHA512256_BLOCK_SIZE = 256;

		[JsonProperty("reseedImplemented")]
		public bool ReseedImplemented { get; private set; }

		[JsonProperty("predResistanceEnabled")]
		public List<bool> PredictionResistance { get; private set; } = new List<bool>();

		[JsonProperty("capabilities")]
		public List<ModeParameters> Modes { get; private set; } = new List<ModeParameters>();

		public Hash(Dictionary<string, string> options) : base("hashDRBG")
		{
			//Prereqs
			PreReqs.Add(BuildPrereq("SHS", options.GetValue("HashDRBG_Prerequisite_SHA")));

			//Reseed
			ReseedImplemented = options.GetValue("Hash_DRBG NO RESEED") == "False";

			//Predication resistance
			if (options.GetValue("Hash_DRBG PREDICTION RESISTANCE ENABLED") == "True") PredictionResistance.Add(true);
			if (options.GetValue("Hash_DRBG PREDICTION RESISTANCE NOT ENABLED") == "True") PredictionResistance.Add(false);

			//Grab the # of output blocks, will need this in each mode, have to do math to get the right value
			int blocks = ParsingHelper.ParseValueToInteger(options.GetValue("Hash_DRBG NumberOfOutputBlocks"));

			//Modes
			if (options.GetValue("Hash_DRBG SHA-1") == "True") Modes.Add(new ModeParameters("SHA-1", options.GetValue("Hash_DRBG SHA-1 lengths"), blocks, SHA1_BLOCK_SIZE));
			if (options.GetValue("Hash_DRBG SHA-224") == "True") Modes.Add(new ModeParameters("SHA2-224", options.GetValue("Hash_DRBG SHA-224 lengths"), blocks, SHA224_BLOCK_SIZE));
			if (options.GetValue("Hash_DRBG SHA-256") == "True") Modes.Add(new ModeParameters("SHA2-256", options.GetValue("Hash_DRBG SHA-256 lengths"), blocks, SHA256_BLOCK_SIZE));
			if (options.GetValue("Hash_DRBG SHA-384") == "True") Modes.Add(new ModeParameters("SHA2-384", options.GetValue("Hash_DRBG SHA-384 lengths"), blocks, SHA384_BLOCK_SIZE));
			if (options.GetValue("Hash_DRBG SHA-512") == "True") Modes.Add(new ModeParameters("SHA2-512", options.GetValue("Hash_DRBG SHA-512 lengths"), blocks, SHA512_BLOCK_SIZE));
			if (options.GetValue("Hash_DRBG SHA-512_224") == "True") Modes.Add(new ModeParameters("SHA2-512/224", options.GetValue("Hash_DRBG SHA-512_224 lengths"), blocks, SHA512224_BLOCK_SIZE));
			if (options.GetValue("Hash_DRBG SHA-512_224") == "True") Modes.Add(new ModeParameters("SHA2-512/256", options.GetValue("Hash_DRBG SHA-512_256 lengths"), blocks, SHA512256_BLOCK_SIZE));
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.HashDRBG
		{
			ReseedImplemented = ReseedImplemented,
			PredictionResistanceEnabled = PredictionResistance,
			Capabilities = Modes.Select(x => new HashDRBG.Capability
			{
				Mode = x.Mode,
				AdditionalInputLength = x.AdditionalInputLength.ToCoreDomain(),
				EntropyInputLength = x.EntropyInputLength.ToCoreDomain(),
				NonceLength = x.NonceLength.ToCoreDomain(),
				PersonalizationStringLength = x.PersonalizationStringLength.ToCoreDomain(),
				ReturnedBitsLength = x.ReturnedBitsLength
			}).ToList()
		};
	}
}
