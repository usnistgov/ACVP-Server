using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class KDF_IKEv1 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public KDF_IKEv1()
		{
			Name = "kdf-components";
			Mode = "ikev1";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("authenticationMethod")]
			public string AuthenticationMethod { get; set; }

			[JsonPropertyName("initiatorNonceLength")]
			public Domain InitiatorNonceLength { get; set; }

			[JsonPropertyName("responderNonceLength")]
			public Domain ResponderNonceLength { get; set; }

			[JsonPropertyName("preSharedKeyLength")]
			public Domain PresharedKeyLength { get; set; }

			[JsonPropertyName("diffieHellmanSharedSecretLength")]
			public Domain DiffieHellmanSharedSecretLength { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}
}
