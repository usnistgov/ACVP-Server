using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA
{
    public class EncryptionResult : ICryptoResult
    {
        public BigInteger CipherText { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public EncryptionResult(BigInteger ct)
        {
            CipherText = ct;
        }

        public EncryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
