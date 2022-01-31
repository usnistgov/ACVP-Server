using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.Core;

namespace NIST.CVP.ACVTS.Libraries.Generation.Core
{
    public class MultipleTestCaseGenerateResponse<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup<TTestGroup, TTestCase>
        where TTestCase : ITestCase<TTestGroup, TTestCase>
    {
        public IEnumerable<TTestCase> TestCases { get; }
        public string ErrorMessage { get; }

        public MultipleTestCaseGenerateResponse(IEnumerable<TTestCase> testCases)
        {
            TestCases = testCases;
        }

        public MultipleTestCaseGenerateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
