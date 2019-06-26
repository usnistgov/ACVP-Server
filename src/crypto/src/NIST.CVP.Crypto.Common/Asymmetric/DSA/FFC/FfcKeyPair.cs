using System.Numerics;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    /// <summary>
    /// A FFC DSA private/public key pair
    /// </summary>
    public class FfcKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Private Key X. Range [1, <see cref="FfcDomainParameters.Q"/> – 1]
        /// </summary>
        public BitString PrivateKeyX { get; set; }
        
        /// <summary>
        /// Public Key Y.  Range [1, <see cref="FfcDomainParameters.P"/> - 1]
        /// </summary>
        public BitString PublicKeyY { get; set; }

        public FfcKeyPair(BitString privateKeyX, BitString publicKeyY)
        {
            PrivateKeyX = privateKeyX;
            PublicKeyY = publicKeyY;
        }

        public FfcKeyPair(BitString publicKeyY)
        {
            PublicKeyY = publicKeyY;
        }

        public FfcKeyPair()
        {
            
        }
    }
}