using System.Numerics;

namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// A FFC DSA private/public key pair
    /// </summary>
    public class FfcDsaKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Private Key X. Range [1, <see cref="FfcDomainParameters.Q"/> – 1]
        /// </summary>
        public BigInteger PrivateKeyX { get; }
        
        /// <summary>
        /// Public Key Y.  Range [1, <see cref="FfcDomainParameters.P"/> - 1]
        /// </summary>
        public BigInteger PublicKeyY { get; }

        public FfcDsaKeyPair(BigInteger privateKeyX, BigInteger publicKeyY)
        {
            PrivateKeyX = privateKeyX;
            PublicKeyY = publicKeyY;
        }
    }
}