using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Tests.Core.Fakes
{
    public class FakeStaticTestCaseGenerator<TTestGroup, TTestCase> : IKnownAnswerTestCaseGenerator<TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase, new()
    {
        public MultipleTestCaseGenerateResponse<TTestCase> Generate(TTestGroup testGroup)
        {
            List<TTestCase> list = new List<TTestCase>();

            return new MultipleTestCaseGenerateResponse<TTestCase>(list);
        }
    }
}
