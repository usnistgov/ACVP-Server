using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class CounterDRBG : PersistedAlgorithmBase
	{
		[AlgorithmProperty("predResistance")]
		public List<bool> PredictionResistanceEnabled { get; set; }

		[AlgorithmProperty("reseed")]
		public bool ReseedImplemented { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public CounterDRBG()
		{
			Name = "ctrDRBG";
			Revision = "1.0";
		}

		public CounterDRBG(ACVPCore.Algorithms.External.CounterDRBG external) : this()
		{
			PredictionResistanceEnabled = external.PredictionResistanceEnabled;
			ReseedImplemented = external.ReseedImplemented;
			foreach(var externalCapability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					Mode = externalCapability.Mode,
					DerivationFunctionEnabled = externalCapability.DerivationFunctionEnabled,
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

			[AlgorithmProperty("derFuncEnabled")]
			public bool DerivationFunctionEnabled { get; set; }

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
