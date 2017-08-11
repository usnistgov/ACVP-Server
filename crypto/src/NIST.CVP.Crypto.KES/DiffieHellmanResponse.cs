using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    public class DiffieHellmanResponse
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

        public DiffieHellmanResponse(BitString sharedSecretZ)
        {
            SharedSecretZ = sharedSecretZ;
        }

        public DiffieHellmanResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}