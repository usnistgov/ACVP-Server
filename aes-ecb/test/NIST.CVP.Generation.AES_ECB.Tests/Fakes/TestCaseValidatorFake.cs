using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB.Tests.Fakes
{
    public class TestCaseValidatorFake : ITestCaseValidator
    {
        public int TestCaseId { get; set; }
        private readonly string _result;
        public TestCaseValidatorFake(string result)
        {
            _result = result;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            return new TestCaseValidation {TestCaseId = TestCaseId, Result = _result};
        }
    }
}
