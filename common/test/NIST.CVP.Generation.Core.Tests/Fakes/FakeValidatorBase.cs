using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Parsers;

namespace NIST.CVP.Generation.Core.Tests.Fakes
{
    public class FakeSuccessValidatorBase : ValidatorBase
    {

        public FakeSuccessValidatorBase(IDynamicParser iDynamicParser)
        {
            _dynamicParser = iDynamicParser;
        }

        public override TestVectorValidation ValidateWorker(ParseResponse<object> answerParseResponse, ParseResponse<object> promptParseResponse,
            ParseResponse<object> testResultParseResponse)
        {
            return new TestVectorValidation()
            {
                Validations = new List<TestCaseValidation>()
                {
                    new TestCaseValidation()
                    {
                        TestCaseId = 1,
                        Reason = string.Empty,
                        Result = Disposition.Passed
                    }
                }
            };
        }
    }
}
