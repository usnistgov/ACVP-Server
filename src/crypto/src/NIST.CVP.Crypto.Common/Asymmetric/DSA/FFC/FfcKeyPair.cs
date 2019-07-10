using System.Numerics;

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
        public BigInteger PrivateKeyX { get; set; }
        
        /// <summary>
        /// Public Key Y.  Range [1, <see cref="FfcDomainParameters.P"/> - 1]
        /// </summary>
        public BigInteger PublicKeyY { get; set; }

        public FfcKeyPair(BigInteger privateKeyX, BigInteger publicKeyY)
        {
            PrivateKeyX = privateKeyX;
            PublicKeyY = publicKeyY;
        }

        public FfcKeyPair(BigInteger publicKeyY)
        {
            PublicKeyY = publicKeyY;
        }

        public FfcKeyPair()
        {
            
        }
    }
}