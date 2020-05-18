using System.Text.Json.Serialization;

namespace NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models
{
	public class TestSessionKeepAlivePayload : IMessagePayload
	{
		[JsonPropertyName("tsId")]
		public long TestSessionID { get; set; }
	}
}
