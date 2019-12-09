using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Math.Exceptions;
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

        public ValidateResponse Validate(ValidateRequest validateRequest)
        {
            TestVectorValidation response;
            try
            {
                response = ValidateWorker(validateRequest.ResultJson, validateRequest.InternalJson, validateRequest.ShowExpected);
            }
            catch (FileNotFoundException ex)
            {
                ThisLogger.Error(ex, $"ERROR in Validator. Unable to find file.");
                return new ValidateResponse(ex.Message, StatusCode.FileReadError);
            }
            catch (InvalidBitStringLengthException ex)
            {
                ThisLogger.Error(ex, $"ERROR in Validator. Failed parsing");
                return new ValidateResponse(ex.Message, StatusCode.BitStringParseError);
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex, "ERROR in Validator.");
                ThisLogger.Error(ex.ToString());
                return new ValidateResponse(ex.Message, StatusCode.TestCaseValidatorError);
            }

            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new ValidationContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            return new ValidateResponse(validationJson);
        }

        protected virtual TestVectorValidation ValidateWorker(string testResultText, string answerText, bool showExpected)
        {
            var results = _vectorSetDeserializer.Deserialize(testResultText);
            var answers = _vectorSetDeserializer.Deserialize(answerText);

            if (results == null)
            {
                throw new Exception("Unable to parse results file.");
            }

            if (answers == null)
            {
                throw new Exception("Unable to parse internalProjection file.");
            }

            var testCaseValidators = _testCaseValidatorFactory.GetValidators(answers);

            if (testCaseValidators == null || !testCaseValidators.Any())
            {
                throw new Exception("Unable to initialize validators for provided vector set.");
            }

            var response = _resultValidator.ValidateResults(testCaseValidators, results.TestGroups, showExpected);

            response.VectorSetId = answers.VectorSetId;

            return response;
        }
        
        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}