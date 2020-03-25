using System.Collections.Generic;
using NIST.CVP.Algorithms.DataTypes;
using External = NIST.CVP.Algorithms.External;

namespace NIST.CVP.Algorithms.Persisted
{
	public class KDF_IKEv1 : PersistedAlgorithmBase
	{
		[AlgorithmProperty("capabilities")]
		public List<Capability> Capabilities { get; set; } = new List<Capability>();

		public KDF_IKEv1()
		{
			Name = "kdf-components";
			Mode = "ikev1";
			Revision = "1.0";
		}

		public KDF_IKEv1(External.KDF_IKEv1 external) : this()
		{
			foreach (var capability in external.Capabilities)
			{
				Capabilities.Add(new Capability
				{
					AuthenticationMethod = capability.AuthenticationMethod,
					InitiatorNonceLength = capability.InitiatorNonceLength,
					ResponderNonceLength = capability.ResponderNonceLength,
					PresharedKeyLength = capability.PresharedKeyLength,
					DiffieHellmanSharedSecretLength = capability.DiffieHellmanSharedSecretLength,
					HashAlgorithms = capability.HashAlgorithms
				});
			}
		}

		public class Capability
		{
			[AlgorithmProperty("authenticationMethod")]
			public string AuthenticationMethod { get; set; }

			[AlgorithmProperty("initiatorNonceLength")]
			public Domain InitiatorNonceLength { get; set; }

			[AlgorithmProperty("responderNonceLength")]
			public Domain ResponderNonceLength { get; set; }

			[AlgorithmProperty("preSharedKeyLength")]
			public Domain PresharedKeyLength { get; set; }

			[AlgorithmProperty("diffieHellmanSharedSecretLength")]
			public Domain DiffieHellmanSharedSecretLength { get; set; }

			[AlgorithmProperty("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}

	
}
