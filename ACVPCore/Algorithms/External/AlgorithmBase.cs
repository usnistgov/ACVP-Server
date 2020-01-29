using System.Text.Json.Serialization;

namespace ACVPCore.Algorithms.External
{
	public class AlgorithmBase
	{
		[JsonPropertyName("algorithm")]
		public string Name { get; set; }

		[JsonPropertyName("mode")]
		public string Mode { get; set; }

		[JsonPropertyName("revision")]
		public string Revision { get; set; }
	}
}
