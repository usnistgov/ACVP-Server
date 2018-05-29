using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.DSA.FFC.PQGVer
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        private readonly TestCase _expectedResult;
        public int TestCaseId => _expectedResult.TestCaseId;

        public TestCaseValidator(TestCase expectedResult)
        {
            _expectedResult = expectedResult;
        }

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            if (_expectedResult.TestPassed != suppliedResult.TestPassed)
            {
                var expected = new Dictionary<string, string>();
                expected.Add(nameof(_expectedResult.TestPassed), _expectedResult.TestPassed.Value.ToString());

                var provided = new Dictionary<string, string>();
                provided.Add(nameof(suppliedResult.TestPassed), suppliedResult.TestPassed.Value.ToString());

                return new TestCaseValidation
                {
                    TestCaseId = suppliedResult.TestCaseId,
                    Result = Core.Enums.Disposition.Failed,
                    Reason = _expectedResult.Reason,
                    Expected = expected.Count != 0 && showExpected ? expected : null,
                    Provided = provided.Count != 0 && showExpected ? provided : null
                };
            }

            return new TestCaseValidation { TestCaseId = suppliedResult.TestCaseId, Result = Core.Enums.Disposition.Passed };
        }
    }
}
