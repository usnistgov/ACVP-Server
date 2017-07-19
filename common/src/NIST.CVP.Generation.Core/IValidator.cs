namespace NIST.CVP.Generation.Core
{
    public interface IValidator
    {
        ValidateResponse Validate(string resultPath, string answerPath, string promptPath);
    }
}