using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.Core
{
    public class TestCaseGenerateResponse
    {
        public ITestCase TestCase { get; private set; }
        public string ErrorMessage { get; private set; }

        public TestCaseGenerateResponse(ITestCase testCase)
        {
            TestCase = testCase;
        }

        public TestCaseGenerateResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success
        {
            get { return string.IsNullOrEmpty(ErrorMessage); }
        }
    }
}
