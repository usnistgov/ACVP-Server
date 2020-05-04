using System.Text.Json;
using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads
{
	public class SubmitResultsPayload
	{
		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }

		[JsonPropertyName("algorithmId")]
		public long AlgorithmID { get; set; }

		[JsonPropertyName("testGroups")]
		public JsonElement Results { get; set; }
	}
}
