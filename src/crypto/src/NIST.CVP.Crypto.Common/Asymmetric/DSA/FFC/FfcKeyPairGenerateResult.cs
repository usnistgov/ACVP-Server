namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class FfcKeyPairGenerateResult : IKeyPairGenerateResult
    {
        public FfcKeyPair KeyPair { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public FfcKeyPairGenerateResult(FfcKeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public FfcKeyPairGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
