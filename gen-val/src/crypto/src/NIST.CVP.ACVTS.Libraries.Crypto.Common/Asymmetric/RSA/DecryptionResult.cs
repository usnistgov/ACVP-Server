using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA
{
    public class DecryptionResult : ICryptoResult
    {
        public BigInteger PlainText { get; }
        public string ErrorMessage { get; }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        public DecryptionResult(BigInteger pt)
        {
            PlainText = pt;
        }

        public DecryptionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
