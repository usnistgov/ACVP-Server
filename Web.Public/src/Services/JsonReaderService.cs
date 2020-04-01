using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Serilog;
using Web.Public.JsonObjects;

namespace Web.Public.Services
{
    public class JsonReaderService : IJsonReaderService
    {
        public T GetObjectFromBodyJson<T>(string jsonBody)
            where T : IJsonObject
        {
            try
            {
                // Unwrap the array
                var jsonObjects = JsonSerializer.Deserialize<object[]>(jsonBody);
                
                if (jsonObjects.Length != 2)
                {
                    throw new Exception("Unable to deserialize body into two objects");
                }

                // Extract and verify version info
                var versionObject = JsonSerializer.Deserialize<VersionObject>(jsonObjects[0].ToString());
                if (versionObject.AcvVersion != "1.0")
                {
                    throw new Exception($"Invalid version provided: {versionObject.AcvVersion}");
                }

                // Extract and verify object info
                var extractedObject = JsonSerializer.Deserialize<T>(jsonObjects[1].ToString());
                
                // Perform any object validation
                var errorList = extractedObject.ValidateObject();
                if (errorList.Any())
                {
                    throw new Exception("Errors parsing body object");
                }

                return extractedObject;
            }
            catch (Exception ex)
            {
                Log.Error("Unable to parse json", ex);
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