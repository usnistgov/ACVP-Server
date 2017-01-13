using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    public interface IStaticTestCaseGenerator<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : IEnumerable<ITestCase>
    {
        MultipleTestCaseGenerateResponse<TTestCase> Generate(TTestGroup testGroup);
    }
}