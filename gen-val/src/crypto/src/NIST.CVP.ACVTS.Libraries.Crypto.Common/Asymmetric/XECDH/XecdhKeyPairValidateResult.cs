namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH
{
    public class XecdhKeyPairValidateResult : ICryptoResult
    {
        /// <summary>
        /// Was the validation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Message associated to validation attempt
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public XecdhKeyPairValidateResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public XecdhKeyPairValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
