using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.Core.Async;

namespace NIST.CVP.Generation.KDF_Components.v1_0.ANSIX942
{
    public class TestCaseValidator : ITestCaseValidatorAsync<TestGroup, TestCase>
    {
        private readonly TestCase _expectedCase;
        public int TestCaseId => _expectedCase.TestCaseId;

        public TestCaseValidator(TestCase expectedCase)
        {
            _expectedCase = expectedCase;
        }

        public Task<TestCaseValidation> ValidateAsync(TestCase suppliedResult, bool showExpected = false)
        {
            throw new NotImplementedException();
        }
    }
}
