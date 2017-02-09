using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface IKnownAnswerTestCaseGenerator<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        MultipleTestCaseGenerateResponse<TTestCase> Generate(TTestGroup testGroup);
    }
}