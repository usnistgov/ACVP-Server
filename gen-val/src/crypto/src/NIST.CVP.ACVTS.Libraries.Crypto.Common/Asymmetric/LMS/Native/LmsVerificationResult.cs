namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Native;

public class LmsVerificationResult : ICryptoResult
{
    /// <summary>
    /// Was the generation successful?
    /// </summary>
    public bool Success => string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Message associated to generation attempt
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// No errors
    /// </summary>
    public LmsVerificationResult() { }

    /// <summary>
    /// Include error message
    /// </summary>
    /// <param name="errorMessage"></param>
    public LmsVerificationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
