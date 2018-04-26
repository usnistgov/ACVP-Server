using System;
using System.IO;
using Newtonsoft.Json;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NLog;

namespace NIST.CVP.Generation.Core
{
    public class Validator<TTestVectorSet, TTestGroup, TTestCase> : IValidator
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        protected readonly IResultValidator<TTestGroup, TTestCase> _resultValidator;
        protected readonly ITestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase> _testCaseValidatorFactory;
        protected readonly IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> _vectorSetDeserializer;

        public Validator(
            IResultValidator<TTestGroup, TTestCase> resultValidator, 
            ITestCaseValidatorFactory<TTestVectorSet, TTestGroup, TTestCase> testCaseValidatorFactory, 
            IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> vectorSetDeserializer
        )
        {
            _resultValidator = resultValidator;
            _testCaseValidatorFactory = testCaseValidatorFactory;
            _vectorSetDeserializer = vectorSetDeserializer;
        }

        public ValidateResponse Validate(string resultPath, string answerPath)
        {
            var resultText = ReadFromFile(resultPath);
            var answerText = ReadFromFile(answerPath);

            var response = ValidateWorker(resultText, answerText);

            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new ValidationContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            var saveResult = SaveToFile(Path.GetDirectoryName(resultPath), "validation.json", validationJson);

            if (!string.IsNullOrEmpty(saveResult))
            {
                return new ValidateResponse(saveResult);
            }

            return new ValidateResponse();
        }

        protected virtual TestVectorValidation ValidateWorker(string testResultText, string answerText)
        {
            var results = _vectorSetDeserializer.Deserialize(testResultText);
            var answers = _vectorSetDeserializer.Deserialize(answerText);

            var testCaseValidators = _testCaseValidatorFactory.GetValidators(answers);
            var response = _resultValidator.ValidateResults(testCaseValidators, results.TestGroups);

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
