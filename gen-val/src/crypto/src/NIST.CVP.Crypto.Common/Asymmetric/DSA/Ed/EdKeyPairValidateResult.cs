namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPairValidateResult : IKeyPairValidateResult
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
        public EdKeyPairValidateResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public EdKeyPairValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}