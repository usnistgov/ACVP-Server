namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed
{
    public class EdKeyPairGenerateResult : IKeyPairGenerateResult, ICryptoResult
    {
        public EdKeyPair KeyPair { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public EdKeyPairGenerateResult(EdKeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public EdKeyPairGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
