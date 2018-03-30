namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcSignatureResult : IDsaSignatureResult
    {
        public FfcSignature Signature { get; }
        public string ErrorMessage { get; }

        public FfcSignatureResult(FfcSignature signature)
        {
            Signature = signature;
        }

        public FfcSignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
