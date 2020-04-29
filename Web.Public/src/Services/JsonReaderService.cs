using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        private readonly IMessagePayloadValidatorFactory _workflowItemValidatorFactory;

        public JsonReaderService(IMessagePayloadValidatorFactory workflowItemValidatorFactory)
        {
            _workflowItemValidatorFactory = workflowItemValidatorFactory;
        }
        
        public T GetObjectFromBodyJson<T>(string jsonBody) where T : IJsonObject
        {
            try
            {
                // Unwrap the array
                var jsonObjects = JsonSerializer.Deserialize<object[]>(jsonBody);
                
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
                    throw new JsonReaderException(errorList);
                }

                return extractedObject;
            }
            catch (JsonReaderException ex)
            {
                Log.Error("Error parsing JSON", ex);
                throw;
            }
        }

        public T GetMessagePayloadFromBodyJson<T>(string jsonBody, APIAction apiAction)
            where T : IMessagePayload
        {
            try
            {
                // Unwrap the array
                var jsonObjects = JsonSerializer.Deserialize<object[]>(jsonBody);
                
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

                return extractedObject;
            }
            catch (JsonReaderException ex)
            {
                Log.Error("Error parsing JSON", ex);
                throw;
            }
        }
        
        public string GetJsonFromBody(Stream body)
        {
            var reader = new StreamReader(body, Encoding.UTF8);
            return reader.ReadToEndAsync().Result;
        }
    }
}