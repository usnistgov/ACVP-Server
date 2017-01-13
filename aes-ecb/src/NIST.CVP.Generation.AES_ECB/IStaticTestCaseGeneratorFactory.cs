using System.Collections.Generic;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface IStaticTestCaseGeneratorFactory<in TTestGroup, TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : IEnumerable<ITestCase>
    {
        IStaticTestCaseGenerator<TTestGroup, TTestCase> GetStaticCaseGenerator(string direction, string testType);
    }
}