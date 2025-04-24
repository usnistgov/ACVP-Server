using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.Core.Async;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;

namespace NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;

public class TestCaseGeneratorFactory : ITestCaseGeneratorFactoryAsync<TestGroup, TestCase>
{
    private readonly IOracle _oracle;
        
    public TestCaseGeneratorFactory(IOracle oracle)
    {
        _oracle = oracle;
    }
        
    public ITestCaseGeneratorAsync<TestGroup, TestCase> GetCaseGenerator(TestGroup testGroup)
    {
        if (testGroup.Direction == BlockCipherDirections.Encrypt)
        {
            return new TestCaseGeneratorEncrypt(_oracle);
        }
        else if(testGroup.Direction == BlockCipherDirections.Decrypt)
        {
            return new TestCaseGeneratorDecrypt(_oracle);
        }
        return null;
    }
}
