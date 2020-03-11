using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class KDF : PersistedAlgorithmBase
	{
		[AlgorithmProperty("kGenSP")]
		public List<string> KGenSP { get; set; }

		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public KDF()
		{
			Name = "KDF";
			Revision = "1.0";
		}

		public KDF(ACVPCore.Algorithms.External.KDF external) : this()
		{
			//KGenSP is really old, CAVS doesn't even use it now

			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					KDFMode = capability.KDFMode,
					MacMode = capability.MacMode,
					SupportedLengths = capability.SupportedLengths,
					FixedDataOrder = capability.FixedDataOrder,
					CounterLength = capability.CounterLength,
					SupportsEmptyIV = capability.SupportsEmptyIV
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty("kdfMode")]
			public string KDFMode { get; set; }

			[AlgorithmProperty("macMode")]
			public List<string> MacMode { get; set; }

			[AlgorithmProperty("supportedLengths")]
			public Domain SupportedLengths { get; set; }

			[AlgorithmProperty("fixedDataOrder")]
			public List<string> FixedDataOrder { get; set; }

			[AlgorithmProperty("counterLength")]
			public List<int> CounterLength { get; set; }

			[AlgorithmProperty("supportsEmptyIv")]
			public bool? SupportsEmptyIV { get; set; }
		}
	}

	
}
