using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class RetryObject
    {
        [JsonPropertyName("retry")]
        public int Retry { get; set; } = 30;
    }
}