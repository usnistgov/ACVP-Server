namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccVerificationResult : IDsaVerificationResult, ICryptoResult
    {
        /// <summary>
        /// Was the generation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Message associated to generation attempt
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public EccVerificationResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public EccVerificationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
