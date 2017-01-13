using System.Security.Cryptography.X509Certificates;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_ECB
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator<TestGroup, TestCase> GetCaseGenerator(string direction, string testType);
    }
}