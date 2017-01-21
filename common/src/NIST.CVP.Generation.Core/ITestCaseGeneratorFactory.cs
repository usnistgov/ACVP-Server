using System.Security.Cryptography.X509Certificates;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.Core
{
    public interface ITestCaseGeneratorFactory<in TTestGroup, in TTestCase>
        where TTestGroup : ITestGroup
        where TTestCase : ITestCase
    {
        ITestCaseGenerator<TTestGroup, TTestCase> GetCaseGenerator(TTestGroup testGroup);
    }
}