using System.Numerics;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccSignatureResult : IDsaSignatureResult
    {
        public EccSignature Signature { get; }
        public BigInteger K { get; }
        public string ErrorMessage { get; }

        public EccSignatureResult( BigInteger k, EccSignature signature)
        {
            K = k;
            Signature = signature;
        }

        public EccSignatureResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public bool Success => string.IsNullOrEmpty(ErrorMessage);
    }
}
