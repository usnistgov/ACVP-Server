namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public class VerifyResult
    {
        public string ErrorMessage { get; }

        public VerifyResult() { }

        public VerifyResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
