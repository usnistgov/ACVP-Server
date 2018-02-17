using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.Parsers;
using NIST.CVP.Generation.Core.Resolvers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase> : IGenerator
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        protected readonly ITestVectorFactory<TParameters> _testVectorFactory;
        protected readonly IParameterParser<TParameters> _parameterParser;
        protected readonly IParameterValidator<TParameters> _parameterValidator;
        protected readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> _testCaseGeneratorFactoryFactory;
        protected readonly IList<JsonConverter> _jsonConverters = new List<JsonConverter>();

        public readonly List<JsonOutputDetail> JsonOutputs = new List<JsonOutputDetail>
        {
            new JsonOutputDetail { OutputFileName = "answer.json", Resolver = new AnswerResolver()},
            new JsonOutputDetail { OutputFileName = "prompt.json", Resolver = new PromptResolver()},
            new JsonOutputDetail { OutputFileName = "testResults.json", Resolver = new ResultResolver()},
        };

        public Generator(
            ITestVectorFactory<TParameters> testVectorFactory, 
            IParameterParser<TParameters> parameterParser, 
            IParameterValidator<TParameters> parameterValidator, 
            ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> iTestCaseGeneratorFactoryFactory
        )
        {
            _testVectorFactory = testVectorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
            _testCaseGeneratorFactoryFactory = iTestCaseGeneratorFactoryFactory;

            _jsonConverters.Add(new BitstringConverter());
            _jsonConverters.Add(new BigIntegerConverter());
        }

        public virtual GenerateResponse Generate(string requestFilePath)
        {
            var parameterResponse = _parameterParser.Parse(requestFilePath);
            if (!parameterResponse.Success)
            {
                return new GenerateResponse(parameterResponse.ErrorMessage);
            }
            var parameters = parameterResponse.ParsedObject;
            var validateResponse = _parameterValidator.Validate(parameters);
            if (!validateResponse.Success)
            {
                return new GenerateResponse(validateResponse.ErrorMessage);
            }
            var testVector = _testVectorFactory.BuildTestVectorSet(parameters);
            var testCasesResult = _testCaseGeneratorFactoryFactory.BuildTestCases((TTestVectorSet)testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }
            return SaveOutputs(requestFilePath, testVector);
        }

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

        protected Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}
