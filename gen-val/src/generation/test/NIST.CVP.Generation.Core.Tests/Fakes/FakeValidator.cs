using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeSuccessValidator : ValidatorAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeSuccessValidator(
            IOracle oracle,
            IResultValidatorAsync<FakeTestGroup, FakeTestCase> resultValidator, 
            ITestCaseValidatorFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase> testCaseValidatorFactory, 
            IVectorSetDeserializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase> vectorSetDeserializer
        ) : 
            base(oracle, resultValidator, testCaseValidatorFactory, vectorSetDeserializer) { }

        protected override Task<TestVectorValidation> ValidateWorker(string answerText, string testResultText, bool showExpected)
        {
            return Task.FromResult(new TestVectorValidation()
            {
                Validations = new List<TestCaseValidation>()
                {
                    new TestCaseValidation()
                    {
                        TestCaseId = 0,
                        Result = Disposition.Passed
                    }
                }
            });
        }
    }
}
