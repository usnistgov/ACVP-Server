using System.IO;
using System.Text;
using System.Text.Json;
using Web.Public.JsonObjects;

namespace Web.Public.Helpers
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        public static object BuildVersionedObject(object content)
        {
            var versionObject = new VersionObject
            {
                AcvVersion = "1.0"
            };

            using var stream = new MemoryStream();
            using (var writer = new Utf8JsonWriter(stream))
            {
                writer.WriteStartArray();
                JsonSerializer.Serialize(writer, versionObject, _jsonSerializerOptions);
                JsonSerializer.Serialize(writer, content, _jsonSerializerOptions);
                writer.WriteEndArray();
            }

            // This kinda sucks but prevents "double json" results using JsonResult
            return JsonSerializer.Deserialize<object>(Encoding.UTF8.GetString(stream.ToArray()), _jsonSerializerOptions);
        }
    }
}