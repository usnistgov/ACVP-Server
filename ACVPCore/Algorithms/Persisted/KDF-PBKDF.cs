﻿using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class KDF_PBKDF : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public KDF_PBKDF()
		{
			Name = "kdf-components";
			Mode = "pbkdf";
			Revision = "1.0";
		}

		public KDF_PBKDF(ACVPCore.Algorithms.External.KDF_PBKDF external) : this()
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