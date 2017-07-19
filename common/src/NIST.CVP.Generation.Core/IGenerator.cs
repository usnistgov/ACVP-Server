namespace NIST.CVP.Generation.Core
{
    public interface IGenerator
    {
        GenerateResponse Generate(string requestFilePath);
    }
}