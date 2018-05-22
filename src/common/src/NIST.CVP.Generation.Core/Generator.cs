using System;
using System.Collections.Generic;
using System.IO;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class Generator<TParameters, TTestVectorSet, TTestGroup, TTestCase> : IGenerator
        where TParameters : IParameters
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        protected readonly ITestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase> _testVectorFactory;
        protected readonly IParameterParser<TParameters> _parameterParser;
        protected readonly IParameterValidator<TParameters> _parameterValidator;
        protected readonly ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> _testCaseGeneratorFactoryFactory;
        protected readonly IVectorSetSerializer<TTestVectorSet, TTestGroup, TTestCase> _vectorSetSerializer;

        public readonly List<JsonOutputDetail> JsonOutputs = 
            new List<JsonOutputDetail>
        {
            new JsonOutputDetail { FileName = "internalProjection.json", Projection = Projection.Server },
            new JsonOutputDetail { FileName = "prompt.json", Projection = Projection.Prompt },
            new JsonOutputDetail { FileName = "expectedResults.json", Projection = Projection.Result },
        };

        public Generator(
            ITestVectorFactory<TParameters, TTestVectorSet, TTestGroup, TTestCase> testVectorFactory, 
            IParameterParser<TParameters> parameterParser, 
            IParameterValidator<TParameters> parameterValidator, 
            ITestCaseGeneratorFactoryFactory<TTestVectorSet, TTestGroup, TTestCase> iTestCaseGeneratorFactoryFactory,
            IVectorSetSerializer<TTestVectorSet, TTestGroup, TTestCase> vectorSetSerializer
        )
        {
            _testVectorFactory = testVectorFactory;
            _parameterParser = parameterParser;
            _parameterValidator = parameterValidator;
            _testCaseGeneratorFactoryFactory = iTestCaseGeneratorFactoryFactory;
            _vectorSetSerializer = vectorSetSerializer;
        }

        public virtual GenerateResponse Generate(string requestFilePath)
        {
            var parameterResponse = _parameterParser.Parse(requestFilePath);
            if (!parameterResponse.Success)
            {
                return new GenerateResponse(parameterResponse.ErrorMessage, StatusCode.ParameterError);
            }
            var parameters = parameterResponse.ParsedObject;
            var validateResponse = _parameterValidator.Validate(parameters);
            if (!validateResponse.Success)
            {
                return new GenerateResponse(validateResponse.ErrorMessage, StatusCode.ParameterValidationError);
            }
            var testVector = _testVectorFactory.BuildTestVectorSet(parameters);
            var testCasesResult = _testCaseGeneratorFactoryFactory.BuildTestCases(testVector);
            if (!testCasesResult.Success)
            {
                return testCasesResult;
            }
            return SaveOutputs(requestFilePath, testVector);
        }

        protected GenerateResponse SaveOutputs(string requestFilePath, TTestVectorSet testVector)
        {
            var outputDirPath = Path.GetDirectoryName(requestFilePath);
            foreach (var jsonOutput in JsonOutputs)
            {
                var saveResult = SaveProjectionToFile(outputDirPath, testVector, jsonOutput);
                if (!string.IsNullOrEmpty(saveResult))
                {
                    return new GenerateResponse(saveResult, StatusCode.FileSaveError);
                }
            }

            return new GenerateResponse();
        }

        private string SaveProjectionToFile(string outputPath, TTestVectorSet testVectorSet, JsonOutputDetail jsonOutput)
        {
            var json = _vectorSetSerializer.Serialize(testVectorSet, jsonOutput.Projection);

            return SaveToFile(outputPath, jsonOutput.FileName, json);
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
