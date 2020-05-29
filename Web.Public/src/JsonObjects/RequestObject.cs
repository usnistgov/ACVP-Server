using System.Text.Json.Serialization;
using NIST.CVP.Common.Helpers;
using Web.Public.Models;

namespace Web.Public.JsonObjects
{
    public class RequestObject
    {
        [JsonIgnore] public long RequestID { get; set; }
        public string URL => $"/acvp/v1/requests/{RequestID}";
        [JsonIgnore] public RequestStatus Status { get; set; }
        [JsonPropertyName("status")] public string StatusString => EnumHelpers.GetEnumDescriptionFromEnum(Status);
    }
}