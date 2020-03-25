using System.Text.Json.Serialization;

namespace ACVPCore
{
	public class GenerationTask
	{
		[JsonPropertyName("isSample")]
		public bool IsSample { get; set; }

		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }
	}
}
