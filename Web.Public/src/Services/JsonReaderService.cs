using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using Serilog;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Services.MessagePayloadValidators;

namespace Web.Public.Services
{
    public class JsonReaderService : IJsonReaderService
    {
        public async Task<T> GetObjectFromBodyJsonAsync<T>(Stream jsonBody) where T : IJsonObject
        {
            object[] jsonObjects;
            try
            {
                jsonObjects = await JsonSerializer.DeserializeAsync<object[]>(jsonBody);
            }
            catch (Exception e)
            {
                throw new JsonReaderException($"Unable to parse payload into a valid json object. {e.Message}");
            }
            
            if (jsonObjects.Length != 2)
            {
                throw new JsonReaderException("Unable to deserialize body into two objects");
            }

            // Extract and verify version info
            var versionObject = JsonSerializer.Deserialize<VersionObject>(jsonObjects[0].ToString());
            if (versionObject.AcvVersion != "1.0")
            {
                throw new JsonReaderException($"Invalid version provided: {versionObject.AcvVersion}");
            }

            // Extract and verify object info
            var extractedObject = JsonSerializer.Deserialize<T>(jsonObjects[1].ToString());
            var errorList = extractedObject.ValidateObject();
            if (errorList.Any())
            {
                throw new PayloadValidatorException(errorList);
            }

            return extractedObject;
        }

        public async Task<T> GetMessagePayloadFromBodyJsonAsync<T>(Stream jsonBody, APIAction apiAction)
            where T : IMessagePayload
        {
            object[] jsonObjects;
            try
            {
                jsonObjects = await JsonSerializer.DeserializeAsync<object[]>(jsonBody);
            }
            catch (Exception e)
            {
                throw new JsonReaderException($"Unable to parse payload into a valid json object. {e.Message}");
            }

            if (jsonObjects.Length != 2)
            {
                throw new JsonReaderException("Unable to deserialize body into two objects");
            }

            // Extract and verify version info
            var versionObject = JsonSerializer.Deserialize<VersionObject>(jsonObjects[0].ToString());
            if (versionObject.AcvVersion != "1.0")
            {
                throw new JsonReaderException($"Invalid version provided: {versionObject.AcvVersion}");
            }

            return JsonSerializer.Deserialize<T>(jsonObjects[1].ToString());
        }
    }
}