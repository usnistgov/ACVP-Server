using System;
using System.IO;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NLog;

namespace NIST.CVP.Generation.Core.Async
{
    public class ValidatorAsync<TTestVectorSet, TTestGroup, TTestCase> : IValidator
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        protected readonly IResultValidatorAsync<TTestGroup, TTestCase> _resultValidator;
        protected readonly ITestCaseValidatorFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> _testCaseValidatorFactory;
        protected readonly IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> _vectorSetDeserializer;

        public ValidatorAsync(
            IResultValidatorAsync<TTestGroup, TTestCase> resultValidator, 
            ITestCaseValidatorFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> testCaseValidatorFactory, 
            IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> vectorSetDeserializer
        )
        {
            _resultValidator = resultValidator;
            _testCaseValidatorFactory = testCaseValidatorFactory;
            _vectorSetDeserializer = vectorSetDeserializer;
        }

        public ValidateResponse Validate(string resultPath, string answerPath, bool showExpected)
        {
            var resultText = ReadFromFile(resultPath);
            var answerText = ReadFromFile(answerPath);

            TestVectorValidation response;
            try
            {
                response = ValidateWorker(resultText, answerText, showExpected);
            }
            catch (FileNotFoundException ex)
            {
                ThisLogger.Error($"ERROR in Validator. Unable to find file. {ex.StackTrace}");
                return new ValidateResponse(ex.Message, StatusCode.FileReadError);
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"ERROR in Validator: {ex.StackTrace}");
                return new ValidateResponse(ex.Message, StatusCode.TestCaseValidatorError);
            }

            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new ValidationContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            var saveResult = SaveToFile(Path.GetDirectoryName(resultPath), "validation.json", validationJson);

            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ValidateResponse(saveResult, StatusCode.FileSaveError);
            }

            return new ValidateResponse();
        }

        protected virtual TestVectorValidation ValidateWorker(string testResultText, string answerText, bool showExpected)
        {
            var results = _vectorSetDeserializer.Deserialize(testResultText);
            var answers = _vectorSetDeserializer.Deserialize(answerText);

            var testCaseValidators = _testCaseValidatorFactory.GetValidators(answers);
            var response = _resultValidator.ValidateResults(testCaseValidators, results.TestGroups, showExpected);

            response.VectorSetId = answers.VectorSetId;

            return response;
        }
        
        /// <summary>
        /// Returns contents of file as string
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ReadFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            return File.ReadAllText(path);
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

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}