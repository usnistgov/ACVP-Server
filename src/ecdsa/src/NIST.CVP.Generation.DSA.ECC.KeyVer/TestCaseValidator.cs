using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Enums;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer
{
    public class TestCaseValidator : ITestCaseValidator<TestCase>
    {
        private TestCase _expectedResult;
        public int TestCaseId { get { return _expectedResult.TestCaseId; } }

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
