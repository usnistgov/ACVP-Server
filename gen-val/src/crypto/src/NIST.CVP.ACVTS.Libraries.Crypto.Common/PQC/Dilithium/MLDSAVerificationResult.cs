namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;

public class MLDSAVerificationResult : ICryptoResult
{
    public string ErrorMessage { get; }
    public bool Success => string.IsNullOrEmpty(ErrorMessage);
    
    public MLDSAVerificationResult() { }

    public MLDSAVerificationResult(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
