using System.Collections.Generic;
using Newtonsoft.Json;
using NIST.CVP.Libraries.Internal.LCAVPCore.Registration.Algorithms;

namespace NIST.CVP.Libraries.Internal.LCAVPCore.Registration
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
