using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class RSASP1 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("keyFormat")]
		public string KeyFormat { get => "standard"; }

		public RSASP1(Dictionary<string, string> options) : base("RSA", "signaturePrimitive")
		{
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.RSASignaturePrimitive
		{
			KeyFormat = KeyFormat
		};
	}
}
