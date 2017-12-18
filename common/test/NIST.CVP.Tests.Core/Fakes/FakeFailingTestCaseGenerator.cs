using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeFailingTestCaseGenerator<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase, new()
    {
        public int NumberOfTestCasesToGenerate => 1;
        public TestCaseGenerateResponse Generate(TTestGroup group, bool isSample)
        {
            var testCase = new TTestCase();
            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TTestGroup group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse("Fail");
        }
    }
}
