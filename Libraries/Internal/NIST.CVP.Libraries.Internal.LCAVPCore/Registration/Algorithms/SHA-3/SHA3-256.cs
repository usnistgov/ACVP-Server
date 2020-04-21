using System.Collections.Generic;
using NIST.CVP.Libraries.Internal.Algorithms.Persisted;
using Newtonsoft.Json;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms.SHA_3
{
	public class SHA3_256 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "inBit")]
		public bool BitOrientedInput { get; set; } = false;

		[JsonProperty(PropertyName = "inEmpty")]
		public bool IncludeNull { get; set; } = false;

		//[JsonProperty(PropertyName = "digestSize")]
		//public List<int> DigestSize { get; } = new List<int> { 256 };

		public SHA3_256(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHA3-256")
		{
			if (options.GetValue("SHA3_NoNull") == "False") IncludeNull = true;
			if (options.GetValue("SHA3_256_Byte") == "False") BitOrientedInput = true;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Libraries.Internal.Algorithms.Persisted.SHA3_256
		{
			InBit = BitOrientedInput,
			InEmpty = IncludeNull
		};
	}
}
