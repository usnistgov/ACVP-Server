using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface IStaticTestCaseGeneratorFactory<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        IStaticTestCaseGenerator<TTestGroup, TTestCase> GetStaticCaseGenerator(TTestGroup testGroup);
    }
}