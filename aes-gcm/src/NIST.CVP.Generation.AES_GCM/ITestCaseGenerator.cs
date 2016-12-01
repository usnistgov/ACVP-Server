using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_GCM
{
    public interface ITestCaseGenerator
    {
        string IVGen { get; }
        string Direction { get; }
        TestCaseGenerateResponse Generate(TestGroup @group, bool isSample);
        TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase);
    }
}