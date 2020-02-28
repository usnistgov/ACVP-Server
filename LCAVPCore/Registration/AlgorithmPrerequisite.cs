using Newtonsoft.Json;
using System.Collections.Generic;

namespace LCAVPCore.Registration
{
	public class AlgorithmPrerequisite
	{
		[JsonProperty("algorithm")]
		public string Algorithm { get; set; }

		[JsonProperty("prerequisites")]
		public List<Prerequisite> Prerequisites { get; set; } = new List<Prerequisite>();
	}
}
