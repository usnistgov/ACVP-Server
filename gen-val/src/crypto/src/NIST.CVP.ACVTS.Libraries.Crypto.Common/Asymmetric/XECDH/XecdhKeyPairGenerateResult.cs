namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH
{
    public class XecdhKeyPairGenerateResult : ICryptoResult
    {
        public XecdhKeyPair KeyPair { get; }
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage { get; }

        /// <summary>
        /// No errors
        /// </summary>
        public XecdhKeyPairGenerateResult(XecdhKeyPair keyPair)
        {
            KeyPair = keyPair;
        }

        /// <summary>
        /// Include error message
        /// </summary>
        /// <param name="errorMessage"></param>
        public XecdhKeyPairGenerateResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
