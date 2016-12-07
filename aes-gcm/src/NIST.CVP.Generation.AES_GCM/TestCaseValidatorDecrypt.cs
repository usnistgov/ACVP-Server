using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class TestCaseValidatorDecrypt : ITestCaseValidator
    {
        private readonly TestCase _expectedResult;

        public TestCaseValidatorDecrypt(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public int TestCaseId
        {
            get { return _expectedResult.TestCaseId; }
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            var errors = new List<string>();
            if (!_expectedResult.PlainText.Equals(suppliedResult.PlainText))
            {
                errors.Add("Plain Text does not match");
            }
            //// @@@ is AAD sent back in the decrypt results?
            //if (!_expectedResult.AAD.Equals(suppliedResult.AAD))
            //{
            //    errors.Add("AAD does not match");
            //}
            // @@@ need to account for expected failure, need more information on what that looks like
            if (_expectedResult.FailureTest && !suppliedResult.FailureTest)
            {
                errors.Add("Expected tag validation failure");
            }

            if (errors.Count > 0)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "failed", Reason = string.Join("; ", errors) };
            }
            return  new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = "passed"};
        }
    }
}
