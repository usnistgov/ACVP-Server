using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.JsonObjects
{
    public class MessageObject
    {
        [JsonPropertyName("requestId")]
        public long RequestID { get; set; }
        [JsonPropertyName("json")]
        public object Json { get; set; }
    }
}