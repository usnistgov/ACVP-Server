using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.GenValApp.Models;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class ErrorLogger
    {
        private static string SaveToFile(string fileRoot, string fileName, string json)
        {
            var path = Path.Combine(fileRoot, fileName);
            try
            {
                File.WriteAllText(path, json);
                return null;
            }
            catch (Exception)
            {
                return $"Could not create {path}";
            }
        }

        public static void LogError(StatusCode status, string source, string context, string directory)
        {
            var error = new Error
            {
                StatusCode = status,
                Source = source,
                AdditionalInformation = context
            };

            var errorJson = JsonConvert.SerializeObject(error, Formatting.Indented);

            var saveResult = SaveToFile(directory, "error.json", errorJson);
            if (!string.IsNullOrEmpty(saveResult))
            {
                throw new Exception($"Error saving file: {saveResult}");
            }
        }
    }
}
