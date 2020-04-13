using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class JwtRequestObject : IJsonObject
    {
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        public List<string> ValidateObject()
        {
            return new List<string>();
        }
    }
}