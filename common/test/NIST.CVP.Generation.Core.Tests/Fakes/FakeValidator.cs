using System.Collections.Generic;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeSuccessValidator : Validator<FakeTestVectorSet, FakeTestGroup, FakeTestCase>
    {
        public FakeSuccessValidator(
            IDynamicParser dynamicParser, 
            IResultValidator<FakeTestGroup, FakeTestCase> resultValidator, 
            ITestCaseValidatorFactory<FakeTestVectorSet, FakeTestCase> testCaseValidatorFactory, 
            ITestReconstitutor<FakeTestVectorSet, FakeTestGroup> testReconstitutor) : 
            base(dynamicParser, resultValidator, testCaseValidatorFactory, testReconstitutor) { }

        protected override TestVectorValidation ValidateWorker(ParseResponse<dynamic> answerParseResponse, ParseResponse<dynamic> testResultParseResponse)
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
