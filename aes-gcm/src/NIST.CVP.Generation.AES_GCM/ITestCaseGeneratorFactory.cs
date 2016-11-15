namespace NIST.CVP.Generation.AES_GCM
{
    public interface ITestCaseGeneratorFactory
    {
        ITestCaseGenerator GetCaseGenerator(string direction, string ivGen);
    }
}