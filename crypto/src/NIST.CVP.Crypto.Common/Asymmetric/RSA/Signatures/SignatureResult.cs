using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures
{
    public class SignatureResult
    {
        public BitString Signature { get; }
        public string ErrorMessage { get; }

        public SignatureResult(BitString signature)
        {
            Signature = signature;
        }

        public SignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public override string ToString()
        {
            if (!Success)
            {
                return ErrorMessage;
            }

            return $"Signature: {Signature}";
        }
    }
}
