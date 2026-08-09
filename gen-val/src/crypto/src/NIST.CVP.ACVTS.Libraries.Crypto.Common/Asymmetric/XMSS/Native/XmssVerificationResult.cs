namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;

public class XmssVerificationResult : ICryptoResult
{
    /// <summary>
    /// Was the verification successful?
    /// </summary>
    public bool Success => string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Message associated to verification attempt
    /// </summary>
    public string ErrorMessage { get; }

    /// <summary>
    /// No errors
    /// </summary>
    public XmssVerificationResult() { }

    /// <summary>
    /// Include error message
    /// </summary>
    /// <param name="errorMessage"></param>
    public XmssVerificationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
