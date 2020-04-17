using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Web.Public.JsonObjects;
using Web.Public.Models;

namespace Web.Public.Services
{
    public class JsonWriterService : IJsonWriterService
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public JsonWriterService()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true,
            };
            
            // This needs to be set via constructor as Converters only has a getter
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }
        
        public object BuildVersionedObject(object content)
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

        public string BuildRequestObject(long requestId, APIAction apiActionId, long userId, object content)
        {
            var reqObject = new MessageObject
            {
                RequestID = requestId,
                ApiActionID = apiActionId,
                UserID = userId,
                Json = content
            };

            // Must allow null because that's how DELETE requests operate
            return JsonSerializer.Serialize(reqObject, new JsonSerializerOptions
            {
                IgnoreNullValues = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}