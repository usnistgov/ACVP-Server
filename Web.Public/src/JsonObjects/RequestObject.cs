using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class RequestObject
    {
        // TODO might needs to move message/approvedURL to a separate RequestObject class because of how URL is defined
        
        [JsonIgnore]
        public long RequestID { get; set; }
        
        public string URL => $"acvp/v1/requests/{RequestID}";
        public string Message { get; set; }
        public string ApprovedURL { get; set; }
        public RequestStatus Status { get; set; }
    }
}