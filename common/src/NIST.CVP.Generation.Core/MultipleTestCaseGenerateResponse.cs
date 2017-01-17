using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public class MultipleTestCaseGenerateResponse<TTestCase>
        where TTestCase : ITestCase
    {
        public IEnumerable<TTestCase> TestCases { get; private set; }
        public string ErrorMessage { get; private set; }

        public MultipleTestCaseGenerateResponse(IEnumerable<TTestCase> testCases)
        {
            TestCases = testCases;
        }

        public MultipleTestCaseGenerateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }
    }
}