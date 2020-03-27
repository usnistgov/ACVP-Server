using System.IO;
using System.Text;
using System.Text.Json;
using Web.Public.JsonObjects;

namespace Web.Public.Helpers
{
    public static class JsonHelper
    {
        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IgnoreNullValues = true
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
                JsonSerializer.Serialize(writer, versionObject, JsonSerializerOptions);
                JsonSerializer.Serialize(writer, content, JsonSerializerOptions);
                writer.WriteEndArray();
            }

            // This kinda sucks but prevents "double json" results using JsonResult
            return JsonSerializer.Deserialize<object>(Encoding.UTF8.GetString(stream.ToArray()), JsonSerializerOptions);
        }
    }
}