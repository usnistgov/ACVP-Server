namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdSignatureResult : IDsaSignatureResult
    {
        public EdSignature Signature { get; }
        public string ErrorMessage { get; }

        public EdSignatureResult(EdSignature signature)
        {
            Signature = signature;
        }

        public EdSignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
