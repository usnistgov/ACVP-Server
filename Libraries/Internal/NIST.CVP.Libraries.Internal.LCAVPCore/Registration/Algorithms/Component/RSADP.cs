using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.Component
{
	public class RSADP : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("modLen")]
		public int ModLen { get; } = 2048;

		public RSADP(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "decryptionPrimitive")
		{
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.RSADecryptionPrimitive
		{
			ModLength = ModLen
		};
	}
}
