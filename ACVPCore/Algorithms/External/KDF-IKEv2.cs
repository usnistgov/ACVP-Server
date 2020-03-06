using System.Collections.Generic;
using System.Text.Json.Serialization;
using ACVPCore.Algorithms.DataTypes;

namespace ACVPCore.Algorithms.External
{
	public class KDF_IKEv2 : AlgorithmBase, IExternalAlgorithm
	{
		[JsonPropertyName("capabilities")]
		public List<Capability> Capabilities { get; set; }

		public KDF_IKEv2()
		{
			Name = "kdf-components";
			Mode = "ikev2";
			Revision = "1.0";
		}

		public class Capability
		{
			[JsonPropertyName("initiatorNonceLength")]
			public Domain InitiatorNonceLength { get; set; }

			[JsonPropertyName("responderNonceLength")]
			public Domain ResponderNonceLength { get; set; }

			[JsonPropertyName("diffieHellmanSharedSecretLength")]
			public Domain DiffieHellmanSharedSecretLength { get; set; }

			[JsonPropertyName("derivedKeyingMaterialLength")]
			public Domain DerivedKeyingMaterialLength { get; set; }

			[JsonPropertyName("hashAlg")]
			public List<string> HashAlgorithms { get; set; }
		}
	}
}
