using System.Text.Json.Serialization;

namespace MessageQueueProcessor.MessagePayloads
{
	public class SubmitResultsPayload
	{
		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }

		[JsonPropertyName("algorithmId")]
		public long AlgorithmID { get; set; }

		[JsonPropertyName("response")]
		public string Results { get; set; }
	}
}
