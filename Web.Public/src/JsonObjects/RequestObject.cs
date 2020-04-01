using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class RequestObject
    {
        [JsonIgnore]
        public long RequestID { get; set; }
        
        public string URL { get; set; }
        public string Message { get; set; }
        public string ApprovedURL { get; set; }
        public RequestStatus Status { get; set; }
    }
}