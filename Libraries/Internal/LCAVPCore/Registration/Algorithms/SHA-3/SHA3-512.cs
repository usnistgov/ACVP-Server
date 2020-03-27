using System.Collections.Generic;
using NIST.CVP.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.SHA_3
{
	public class SHA3_512 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "inBit")]
		public bool BitOrientedInput { get; set; } = false;

		[JsonProperty(PropertyName = "inEmpty")]
		public bool IncludeNull { get; set; } = false;

		//[JsonProperty(PropertyName = "digestSize")]
		//public List<int> DigestSize { get; } = new List<int> { 512 };

		public SHA3_512(Dictionary<string, string> options, IDataProvider dataProvider) : base(dataProvider, "SHA3-512")
		{
			if (options.GetValue("SHA3_NoNull") == "False") IncludeNull = true;
			if (options.GetValue("SHA3_512_Byte") == "False") BitOrientedInput = true;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new NIST.CVP.Algorithms.Persisted.SHA3_512
		{
			InBit = BitOrientedInput,
			InEmpty = IncludeNull
		};
	}
}
