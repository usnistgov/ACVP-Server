using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Resolvers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class GeneratorBase
    {
        protected readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>();
        
        public GeneratorBase()
        {
            _jsonConverters.Add(new BitstringConverter());
            _jsonConverters.Add(new BigIntegerConverter());
        }

        public readonly List<JsonOutputDetail> JsonOutputs = new List<JsonOutputDetail>
        {
            new JsonOutputDetail { OutputFileName = "answer.json", Resolver = new AnswerResolver()},
            new JsonOutputDetail { OutputFileName = "prompt.json", Resolver = new PromptResolver()},
            new JsonOutputDetail { OutputFileName = "testResults.json", Resolver = new ResultResolver()},
        };

        protected GenerateResponse SaveOutputs(string requestFilePath, ITestVectorSet testVector)
        {
            var outputDirPath = Path.GetDirectoryName(requestFilePath);
            foreach (var jsonOutput in JsonOutputs)
            {
                var saveResult = SaveProjectionToFile(outputDirPath, testVector, jsonOutput);
                if (!string.IsNullOrEmpty(saveResult))
                {
                    return new GenerateResponse(saveResult);
                }
            }

            return new GenerateResponse();
        }

        private string SaveProjectionToFile(string outputPath, ITestVectorSet testVectorSet, JsonOutputDetail jsonOutput)
        {
            //serialize to file
            var json = JsonConvert.SerializeObject(testVectorSet, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = jsonOutput.Resolver,
                    Converters = _jsonConverters,
                    NullValueHandling = NullValueHandling.Ignore
                });
            return SaveToFile(outputPath, jsonOutput.OutputFileName, json);
        }

        private string SaveToFile(string fileRoot, string fileName, string json)
        {
            string path = Path.Combine(fileRoot, fileName);
            try
            {
                File.WriteAllText(path, json);
                return null;
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return $"Could not create {path}";
            }
        }

        protected Logger ThisLogger
        {
            get { return LogManager.GetCurrentClassLogger(); }
        }
    }
}