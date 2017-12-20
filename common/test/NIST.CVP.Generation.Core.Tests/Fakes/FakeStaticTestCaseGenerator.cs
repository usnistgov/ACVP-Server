using System.Collections.Generic;

namespace NIST.CVP.Generation.Core.Tests.Fakes
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
