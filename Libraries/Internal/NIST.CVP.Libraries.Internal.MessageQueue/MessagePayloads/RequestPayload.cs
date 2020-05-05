using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Internal.ACVPWorkflow;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace NIST.CVP.Libraries.Internal.MessageQueue.MessagePayloads
{
	public class RequestPayload
	{
		[JsonPropertyName("requestId")]
		public long RequestID { get; set; }

		[JsonPropertyName("json")]
		public JsonElement Json { get; set; }
	}
}
