using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface IKnownAnswerTestCaseGeneratorFactory<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        IKnownAnswerTestCaseGenerator<TTestGroup, TTestCase> GetStaticCaseGenerator(TTestGroup testGroup);
    }
}