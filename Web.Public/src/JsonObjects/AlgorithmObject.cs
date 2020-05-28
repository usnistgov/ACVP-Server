using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
	public class AlgorithmObject
    {
		[JsonPropertyName("id")]
		public int AlgorithmId { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("mode")]
		public string Mode { get; set; }

		[JsonPropertyName("revision")]
		public string Revision { get; set; }
	}
}