using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.Algorithms.External
{
	public class AlgorithmBase
	{
		[JsonPropertyName("id")]
		public int AlgorithmId { get; set; }
		
		[JsonPropertyName("algorithm")]
		public string Name { get; set; }

		[JsonPropertyName("mode")]
		public string Mode { get; set; }

		[JsonPropertyName("revision")]
		public string Revision { get; set; }
	}
}
