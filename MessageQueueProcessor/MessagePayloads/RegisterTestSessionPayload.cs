using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MessageQueueProcessor.MessagePayloads
{
	public class RegisterTestSessionPayload
	{
		[JsonPropertyName("userId")]
		public long UserID { get; set; }

		[JsonPropertyName("testSessionId")]
		public long TestSessionID { get; set; }

		[JsonPropertyName("acvVersion")]
		public string ACVVersion { get; set; }

		[JsonPropertyName("isSample")]
		public bool IsSample { get; set; }

		[JsonPropertyName("vectorSets")]
		public List<VectorSetRegistration> VectorSetRegistrations { get; set; }
	}

	public class VectorSetRegistration
	{
		[JsonPropertyName("vsId")]
		public long VectorSetID { get; set; }

		[JsonPropertyName("algorithmId")]
		public long AlgorithmID { get; set; }

		[JsonPropertyName("capabilities")]
		public object Capabilities { get; set; }
	}
}
