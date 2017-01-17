using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeTestCaseGenerator<TTestGroup, TTestCase> : ITestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase, new()
    {
        public int NumberOfTestCasesToGenerate { get { return 1; } }
        public TestCaseGenerateResponse Generate(TTestGroup @group, bool isSample)
        {
            TTestCase testCase = new TTestCase();

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TTestGroup @group, TTestCase testCase)
        {
            return new TestCaseGenerateResponse(testCase);
        }
    }
}
