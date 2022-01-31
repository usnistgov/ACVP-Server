using System.Collections.Generic;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Generation.Core.DeSerialization;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core.Tests.Fakes
{
    public class FakeSuccessValidator : ValidatorAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeSuccessValidator(
            IOracle oracle,
            IResultValidatorAsync<FakeTestGroup, FakeTestCase> resultValidator,
            ITestCaseValidatorFactoryAsync<FakeTestVectorSet, FakeTestGroup, FakeTestCase> testCaseValidatorFactory,
            IVectorSetDeserializer<FakeTestVectorSet, FakeTestGroup, FakeTestCase> vectorSetDeserializer
        ) :
            base(oracle, resultValidator, testCaseValidatorFactory, vectorSetDeserializer)
        { }

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
