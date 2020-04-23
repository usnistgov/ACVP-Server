using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions;
using NIST.CVP.Libraries.Shared.ACVPWorkflow.Abstractions.Models;
using Serilog;
using Web.Public.Exceptions;
using Web.Public.JsonObjects;
using Web.Public.Services.WorkflowItemPayloadValidators;

namespace Web.Public.Services
{
    public class JsonReaderService : IJsonReaderService
    {
        private readonly IWorkflowItemValidatorFactory _workflowItemValidatorFactory;

        public JsonReaderService(IWorkflowItemValidatorFactory workflowItemValidatorFactory)
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

        public T GetWorkflowObjectFromBodyJson<T>(string jsonBody, APIAction apiAction)
            where T : IWorkflowItemPayload
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

                var validator = _workflowItemValidatorFactory.GetWorkflowItemPayloadValidator(apiAction);
                var validationResult = validator.Validate(extractedObject);

                if (!validationResult.IsSuccess)
                {
                    throw new JsonReaderException(validationResult.Errors);
                }

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