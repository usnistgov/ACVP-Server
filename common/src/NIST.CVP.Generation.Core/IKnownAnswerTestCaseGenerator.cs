using System;
using System.Collections.Generic;

namespace NIST.CVP.Generation.Core
{
    [Obsolete("TestCaseGenerators using this interface should be updated to utilize the normal ITestCaseGenerator")]
    public interface IKnownAnswerTestCaseGenerator<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        MultipleTestCaseGenerateResponse<TTestCase> Generate(TTestGroup testGroup);
    }
}