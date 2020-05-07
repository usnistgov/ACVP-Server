using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using Web.Public.JsonObjects;

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

        public string BuildRequestWorkflowObject(long requestId, object content)
        {
            var reqObject = new MessageObject
            {
                RequestID = requestId,
                Json = content
            };

            return JsonSerializer.Serialize(reqObject, _jsonSerializerOptions);
        }

        public string BuildMessageObject(object content)
        {
            return JsonSerializer.Serialize(content, _jsonSerializerOptions);
        }
    }
}