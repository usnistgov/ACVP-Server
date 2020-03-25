using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCAVPCore.Registration.Algorithms.DSA.Capabilities
{
	public class SigGenCapability
	{
		[JsonProperty(PropertyName = "l")]
		public int L { get; set; }

		[JsonProperty(PropertyName = "n")]
		public int N { get; set; }

		[JsonProperty(PropertyName = "hashAlg")]
		public List<string> HashAlgorithms { get; set; } = new List<string>();
	}
}
