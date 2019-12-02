using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.GenValApp.Models;
using NLog;
using System;
using System.IO;

namespace NIST.CVP.Generation.GenValApp.Helpers
{
    public static class ErrorLogger
    {
        private static string SaveToFile(string fileRoot, string fileName, string json)
        {
            var path = fileName;
            if (fileRoot != null)
            {
                path = Path.Combine(fileRoot, fileName);
            }
            
            LogManager.GetCurrentClassLogger().Info($"path: {path}");

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
        }
    }
}
