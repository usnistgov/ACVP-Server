using System.Collections.Generic;
using System.Text.Json.Serialization;
using NIST.CVP.Algorithms.DataTypes;

namespace NIST.CVP.Algorithms.External
{
	public class CounterDRBG : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("predResistanceEnabled")]
		public List<bool> PredictionResistanceEnabled { get; set; }

		[JsonPropertyName("reseedImplemented")]
		public bool ReseedImplemented { get; set; }

		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public CounterDRBG()
		{
			Name = "ctrDRBG";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("mode")]
			public string Mode { get; set; }

			[JsonPropertyName("derFuncEnabled")]
			public bool DerivationFunctionEnabled { get; set; }

			[JsonPropertyName("additionalInputLen")]
			public Domain AdditionalInputLength { get; set; }

			[JsonPropertyName("entropyInputLen")]
			public Domain EntropyInputLength { get; set; }

			[JsonPropertyName("nonceLen")]
			public Domain NonceLength { get; set; }

			[JsonPropertyName("persoStringLen")]
			public Domain PersonalizationStringLength { get; set; }

			[JsonPropertyName("returnedBitsLen")]
			public long ReturnedBitsLength { get; set; }
		}
	}
}
