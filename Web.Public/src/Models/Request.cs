using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;

namespace Web.Public.Models
{
    public class Request
    {
        public string Url => $"/acvp/v1/requests/{RequestID}";
        [JsonIgnore]
        public long RequestID { get; set; }
        [JsonIgnore]
        public long ApprovedID { get; set; }
        [JsonIgnore]
        public APIAction APIAction { get; set; }
        
        public RequestStatus Status { get; set; }
        [JsonPropertyName("approvedUrl")]
        public string ApprovedURL { get; set; }
    }
}