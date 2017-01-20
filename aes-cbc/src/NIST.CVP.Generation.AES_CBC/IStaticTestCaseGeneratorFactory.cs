using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CBC
{
    public interface IStaticTestCaseGeneratorFactory<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        IStaticTestCaseGenerator<TTestGroup, TTestCase> GetStaticCaseGenerator(string direction, string testType);
    }
}