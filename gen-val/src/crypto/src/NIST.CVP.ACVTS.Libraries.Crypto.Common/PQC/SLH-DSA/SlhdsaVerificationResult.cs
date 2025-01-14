namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.SLH_DSA;

public class SlhdsaVerificationResult : ICryptoResult
{
    /// <summary>
    /// Was the generation successful?
    /// </summary>
    public bool Success => string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Message associated to generation attempt
    /// </summary>
    private string ErrorMessage { get; }
    
    /// <summary>
    /// No errors
    /// </summary>
    /// <returns></returns>
    public SlhdsaVerificationResult(){ }

    /// <summary>
    /// Include error message
    /// </summary>
    /// <param name="errorMessage"></param>
    public SlhdsaVerificationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
