using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Web.Public.JsonObjects
{
    public class JwtMultiRefreshObject : IJsonObject
    {
    [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("accessToken")]
        public List<string> AccessToken { get; set; }

        public List<string> ValidateObject()
        {
            var errors = new List<string>();
        
            if (string.IsNullOrEmpty(Password))
            {
                errors.Add($"{nameof(JsonObjects.JwtRequestObject)}.{nameof(Password)} is required.");
            }

            return errors;
        }
    }
}