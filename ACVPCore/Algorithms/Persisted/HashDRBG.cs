using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class HashDRBG : PersistedAlgorithmBase
	{
		[AlgorithmProperty("predResistance")]
		public List<bool> PredictionResistanceEnabled { get; set; }

		[AlgorithmProperty("reseed")]
		public bool ReseedImplemented { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public HashDRBG()
		{
			Name = "hashDRBG";
			Revision = "1.0";
		}

		public HashDRBG(ACVPCore.Algorithms.External.HashDRBG external) : this()
		{
			PredictionResistanceEnabled = external.PredictionResistanceEnabled;
			ReseedImplemented = external.ReseedImplemented;
			foreach(var externalCapability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					Mode = externalCapability.Mode,
					AdditionalInputLength = externalCapability.AdditionalInputLength,
					EntropyInputLength = externalCapability.EntropyInputLength,
					NonceLength = externalCapability.NonceLength,
					PersonalizationStringLength = externalCapability.PersonalizationStringLength,
					ReturnedBitsLength = externalCapability.ReturnedBitsLength
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty("capabilities/mode")]
			public string Mode { get; set; }

			[AlgorithmProperty("additionalInput")]
			public Domain AdditionalInputLength { get; set; }

			[AlgorithmProperty("entropyInput")]
			public Domain EntropyInputLength { get; set; }

			[AlgorithmProperty("nonce")]
			public Domain NonceLength { get; set; }

			[AlgorithmProperty("persoString")]
			public Domain PersonalizationStringLength { get; set; }

			[AlgorithmProperty("returnedBits")]
			public long ReturnedBitsLength { get; set; }
		}
	}
}
