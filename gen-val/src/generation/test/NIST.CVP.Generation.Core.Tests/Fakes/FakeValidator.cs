using System.Collections.Generic;
using NIST.CVP.Generation.Core.Async;
using NIST.CVP.Generation.Core.DeSerialization;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeSuccessValidator : ValidatorAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeSuccessValidator(
            IResultValidatorAsync<FakeTestGroup, FakeTestCase> resultValidator, 
            ITestCaseValidatorFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase> testCaseValidatorFactory, 
            IVectorSetDeserializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase> vectorSetDeserializer
        ) : 
            base(resultValidator, testCaseValidatorFactory, vectorSetDeserializer) { }

        protected override TestVectorValidation ValidateWorker(string answerText, string testResultText, bool showExpected)
        {
            return new TestVectorValidation()
            {
                Validations = new List<TestCaseValidation>()
                {
                    new TestCaseValidation()
                    {
                        TestCaseId = 0,
                        Result = Disposition.Passed
                    }
                }
            };
        }
    }
}
