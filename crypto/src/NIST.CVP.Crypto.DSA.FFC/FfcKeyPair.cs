using System.Numerics;

namespace NIST.CVP.Crypto.DSA.FFC
{
    /// <summary>
    /// A FFC DSA private/public key pair
    /// </summary>
    public class FfcKeyPair : IDsaKeyPair
    {
        /// <summary>
        /// Private Key X. Range [1, <see cref="FfcDomainParameters.Q"/> – 1]
        /// </summary>
        public BigInteger PrivateKeyX { get; }
        
        /// <summary>
        /// Public Key Y.  Range [1, <see cref="FfcDomainParameters.P"/> - 1]
        /// </summary>
        public BigInteger PublicKeyY { get; }

        public FfcKeyPair(BigInteger privateKeyX, BigInteger publicKeyY)
        {
            PrivateKeyX = privateKeyX;
            PublicKeyY = publicKeyY;
        }
    }
}