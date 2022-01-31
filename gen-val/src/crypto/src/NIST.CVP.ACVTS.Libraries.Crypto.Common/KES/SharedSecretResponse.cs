using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KES
{
    public class SharedSecretResponse : ICryptoResult
    {
        /// <summary>
        /// The computed shared secret Z
        /// </summary>
        public BitString SharedSecretZ { get; }

        /// <summary>
        /// Was the generation successful?
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);

        /// <summary>
        /// Message associated to generation attempt
        /// </summary>
        public string ErrorMessage { get; }

        public SharedSecretResponse(BitString sharedSecretZ)
        {
            SharedSecretZ = sharedSecretZ;
        }

        public SharedSecretResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
