using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Algorithms.DataTypes;
using External = NIST.CVP.Libraries.Shared.Algorithms.External;

namespace NIST.CVP.Libraries.Internal.Algorithms.Persisted
{
	public class PBKDF : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public PBKDF()
		{
			Name = "PBKDF";
			Revision = "1.0";
		}

		public PBKDF(External.PBKDF external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					IterationCount = capability.IterationCount,
					HMACAlgorithms = capability.HMACAlgorithms,
					PasswordLength = capability.PasswordLength,
					SaltLength = capability.SaltLength,
					KeyLength = capability.KeyLength
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty("iterationCount")]
			public Domain IterationCount { get; set; }

			[AlgorithmProperty("hmacAlg")]
			public List<string> HMACAlgorithms { get; set; }

			[AlgorithmProperty("passwordLen")]
			public Domain PasswordLength { get; set; }

			[AlgorithmProperty("saltLen")]
			public Domain SaltLength { get; set; }

			[AlgorithmProperty("keyLen")]
			public Domain KeyLength { get; set; }
		}
	}

	
}
