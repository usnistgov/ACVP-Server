using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class VersionObject
    {
        [JsonPropertyName("acvVersion")]
        public string AcvVersion { get; set; }
    }
}