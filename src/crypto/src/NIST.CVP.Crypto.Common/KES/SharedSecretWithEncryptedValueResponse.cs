using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KES
{
    public class SharedSecretWithEncryptedValueResponse : SharedSecretResponse
    {
        public BitString Ciphertext { get; }
        
        public SharedSecretWithEncryptedValueResponse(string errorMessage)
            : base(errorMessage)
        {
            
        }

        public SharedSecretWithEncryptedValueResponse(BitString z, BitString ciphertext)
            : base(z)
        {
            Ciphertext = ciphertext;
        }
    }
}