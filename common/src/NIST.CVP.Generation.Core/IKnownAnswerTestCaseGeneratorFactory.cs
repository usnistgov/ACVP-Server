using System;

namespace NIST.CVP.Generation.Core
{
    [Obsolete("use ITestCaseGeneratorFactory instead")]
    public interface IKnownAnswerTestCaseGeneratorFactory<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        IKnownAnswerTestCaseGenerator<TTestGroup, TTestCase> GetStaticCaseGenerator(TTestGroup testGroup);
    }
}