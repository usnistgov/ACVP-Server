using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class HssKeyPairGenerateResult : ICryptoResult
    {
        public BitString PublicKey { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public HssKeyPairGenerateResult(BitString publicKey)
        {
            PublicKey = publicKey;
        }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public HssKeyPairGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
