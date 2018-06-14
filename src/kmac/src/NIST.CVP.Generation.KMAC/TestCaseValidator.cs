using System.Collections.Generic;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KMAC
{
    public class TestCaseValidator : ITestCaseValidator<TestGroup, TestCase>
    {
        public int TestCaseId => throw new System.NotImplementedException();

        public TestCaseValidation Validate(TestCase suppliedResult, bool showExpected = false)
        {
            throw new System.NotImplementedException();
        }
    }
}
