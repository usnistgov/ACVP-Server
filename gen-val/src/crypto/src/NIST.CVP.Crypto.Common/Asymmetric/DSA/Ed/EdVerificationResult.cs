namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdVerificationResult : IDsaVerificationResult, ICryptoResult
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
        public EdVerificationResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public EdVerificationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
