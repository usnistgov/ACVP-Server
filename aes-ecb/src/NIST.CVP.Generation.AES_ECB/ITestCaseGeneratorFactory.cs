namespace NIST.CVP.Generation.AES_ECB
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator GetCaseGenerator(string direction);
    }
}