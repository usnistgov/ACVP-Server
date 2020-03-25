using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.SHA_3
{
	public class SHA3_384 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "inBit")]
		public bool BitOrientedInput { get; set; } = false;

		[JsonProperty(PropertyName = "inEmpty")]
		public bool IncludeNull { get; set; } = false;

		//[JsonProperty(PropertyName = "digestSize")]
		//public List<int> DigestSize { get; } = new List<int> { 384 };

		public SHA3_384(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHA3-384")
		{
			if (options.GetValue("SHA3_NoNull") == "False") IncludeNull = true;
			if (options.GetValue("SHA3_384_Byte") == "False") BitOrientedInput = true;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.SHA3_384
		{
			InBit = BitOrientedInput,
			InEmpty = IncludeNull
		};
	}
}
