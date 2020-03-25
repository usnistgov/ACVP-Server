using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCAVPCore.Registration.Algorithms.ECDSA.Capabilities
{
	public class SigCapability
	{
		[JsonProperty(PropertyName = "curve")]
		public List<string> Curve { get; set; } = new List<string>();

		[JsonProperty(PropertyName = "hashAlg")]
		public List<string> HashAlgorithms { get; set; } = new List<string>();
	}
}
