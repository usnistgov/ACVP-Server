using System.Collections.Generic;
using LCAVPCore.Registration.Algorithms;
using Newtonsoft.Json;

namespace LCAVPCore.Registration
{
	public class AlgorithmCapabilities
	{
		[JsonProperty("algorithm")]
		public AlgorithmContainer AlgorithmChunk { get => new AlgorithmContainer { Algorithm = Algorithm.Algorithm, Mode = Algorithm.Mode, Revision = Algorithm.Revision }; }

		[JsonProperty("capabilities")]
		public IAlgorithm Algorithm { get; set; }

		[JsonProperty("prerequisites", NullValueHandling = NullValueHandling.Ignore)]
		public List<Prerequisite> Prerequisites { get => Algorithm.CleanPreReqs; }

		public AlgorithmCapabilities(IAlgorithm algorithm)
		{
			Algorithm = algorithm;
		}
	}
}
