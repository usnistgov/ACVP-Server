using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.Component
{
	public class RSADP : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("modLen")]
		public int ModLen { get; } = 2048;

		public RSADP(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "decryptionPrimitive")
		{
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.RSADecryptionPrimitive
		{
			ModLength = ModLen
		};
	}
}
