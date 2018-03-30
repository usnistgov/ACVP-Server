namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public class EccKeyPairGenerateResult : IKeyPairGenerateResult
    {
        public EccKeyPair KeyPair { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public EccKeyPairGenerateResult(EccKeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public EccKeyPairGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}