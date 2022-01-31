using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public class HssSignatureGenerateResult : ICryptoResult
    {
        public BitString Signature { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public HssSignatureGenerateResult(BitString sig)
        {
            Signature = sig;
        }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public HssSignatureGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
