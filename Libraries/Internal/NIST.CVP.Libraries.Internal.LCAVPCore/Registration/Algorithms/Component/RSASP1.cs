using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.Component
{
	public class RSASP1 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty("keyFormat")]
		public string KeyFormat { get => "standard"; }

		public RSASP1(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "RSA", "signaturePrimitive")
		{
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.RSASignaturePrimitive
		{
			KeyFormat = KeyFormat
		};
	}
}
