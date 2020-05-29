using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NIST.CVP.Common.Enums;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Crypto.Oracle;
using NIST.CVP.Generation.Core.ContractResolvers;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Math.Exceptions;
using NLog;

namespace NIST.CVP.Generation.Core.Async
{
    public class ValidatorAsync<TTestVectorSet, TTestGroup, TTestCase> : IValidator
        where TTestVectorSet : ITestVectorSet<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        private readonly IOracle _oracle;
        private readonly IResultValidatorAsync<TTestGroup, TTestCase> _resultValidator;
        private readonly ITestCaseValidatorFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> _testCaseValidatorFactory;
        private readonly IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> _vectorSetDeserializer;
        
        public ValidatorAsync(
            IOracle oracle,
            IResultValidatorAsync<TTestGroup, TTestCase> resultValidator, 
            ITestCaseValidatorFactoryAsync<TTestVectorSet, TTestGroup, TTestCase> testCaseValidatorFactory, 
            IVectorSetDeserializer<TTestVectorSet, TTestGroup, TTestCase> vectorSetDeserializer
        )
        {
            _oracle = oracle;
            _resultValidator = resultValidator;
            _testCaseValidatorFactory = testCaseValidatorFactory;
            _vectorSetDeserializer = vectorSetDeserializer;
        }

        public async Task<ValidateResponse> ValidateAsync(ValidateRequest validateRequest)
        {
            TestVectorValidation response;
            try
            {
                await _oracle.InitializeClusterClient();
                response = await ValidateWorker(validateRequest.ResultJson, validateRequest.InternalJson, validateRequest.ShowExpected);
                await _oracle.CloseClusterClient();
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

            var statusCode = response.Disposition == Disposition.Passed ? StatusCode.Success : StatusCode.ValidatorFail;
            var validationJson = JsonConvert.SerializeObject(response, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ContractResolver = new ValidationContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });

            return new ValidateResponse(validationJson, statusCode);
        }

        protected virtual async Task<TestVectorValidation> ValidateWorker(string testResultText, string answerText, bool showExpected)
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

            var response = await _resultValidator.ValidateResultsAsync(testCaseValidators, results.TestGroups, showExpected);

            response.VectorSetId = answers.VectorSetId;

            return response;
        }
        
        private static Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}