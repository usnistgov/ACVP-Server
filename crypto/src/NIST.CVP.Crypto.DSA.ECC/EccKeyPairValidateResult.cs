namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccKeyPairValidateResult : IKeyPairValidateResult
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
        public EccKeyPairValidateResult() { }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public EccKeyPairValidateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}