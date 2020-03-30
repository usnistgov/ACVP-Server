using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Serilog;
using Web.Public.JsonObjects;

namespace Web.Public.Services
{
    public class JsonService<T>
        where T : IJsonObject
    {
        public T GetObjectFromBodyJson(string jsonBody)
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
            var request = body;
            request.Seek(0, SeekOrigin.Begin);
            return new StreamReader(request).ReadToEnd();
        }
    }
}