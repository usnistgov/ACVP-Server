using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;
using NIST.CVP.Generation.Core.Helpers;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }
        private readonly TestCase _expectedResult;

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult)
        {
            if (_expectedResult.Result != suppliedResult.Result)
            {
                return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Failed, Reason = EnumHelpers.GetEnumDescriptionFromEnum(_expectedResult.Reason) };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Disposition.Passed };
        }
    }
}
