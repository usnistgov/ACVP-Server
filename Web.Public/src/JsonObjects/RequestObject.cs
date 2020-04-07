using System.Text.Json.Serialization;
using Web.Public.Models;

namespace Web.Public.JsonObjects
{
    public class RequestObject
    {
        [JsonIgnore]
        public long RequestID { get; set; }
        
        public string URL => $"acvp/v1/requests/{RequestID}";
        public RequestStatus Status { get; set; }
    }
}