using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
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

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.RSADecryptionPrimitive
		{
			ModLength = ModLen
		};
	}
}
