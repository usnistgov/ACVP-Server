using System.Collections.Generic;
using ACVPCore.Algorithms.Persisted;
using Newtonsoft.Json;

namespace LCAVPCore.Registration.Algorithms.SHA_3
{
	public class SHA3_224 : AlgorithmBase, IAlgorithm
	{
		[JsonProperty(PropertyName = "inBit")]
		public bool BitOrientedInput { get; set; } = false;

		[JsonProperty(PropertyName = "inEmpty")]
		public bool IncludeNull { get; set; } = false;

		//[JsonProperty(PropertyName = "digestSize")]
		//public List<int> DigestSize { get; } = new List<int> { 224 };

		public SHA3_224(Dictionary<string, string> options) : base("SHA3-224")
		{
			if (options.GetValue("SHA3_NoNull") == "False") IncludeNull = true;
			if (options.GetValue("SHA3_224_Byte") == "False") BitOrientedInput = true;
		}

		public IPersistedAlgorithm ToPersistedAlgorithm() => new ACVPCore.Algorithms.Persisted.SHA3_224
		{
			InBit = BitOrientedInput,
			InEmpty = IncludeNull
		};
	}
}
