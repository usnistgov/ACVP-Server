using System.Collections.Generic;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.Persisted
{
	public class KDF_IKEv2 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public KDF_IKEv2()
		{
			Name = "kdf-components";
			Mode = "ikev2";
			Revision = "1.0";
		}

		public KDF_IKEv2(ACVPCore.Algorithms.External.KDF_IKEv2 external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					InitiatorNonceLength = capability.InitiatorNonceLength,
					ResponderNonceLength = capability.ResponderNonceLength,
					DiffieHellmanSharedSecretLength = capability.DiffieHellmanSharedSecretLength,
					DerivedKeyingMaterialLength = capability.DerivedKeyingMaterialLength,
					HashAlgorithms = capability.HashAlgorithms
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty("initiatorNonceLength")]
			public Domain InitiatorNonceLength { get; set; }

			[AlgorithmProperty("responderNonceLength")]
			public Domain ResponderNonceLength { get; set; }

			[AlgorithmProperty("diffieHellmanSharedSecretLength")]
			public Domain DiffieHellmanSharedSecretLength { get; set; }

			[AlgorithmProperty("derivedKeyingMaterialLength")]
			public Domain DerivedKeyingMaterialLength { get; set; }

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}

	
}
