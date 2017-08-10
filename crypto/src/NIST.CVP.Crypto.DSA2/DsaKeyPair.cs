using System.Numerics;

namespace NIST.CVP.Crypto.DSA2
{
    /// <summary>
    /// A DSA private/public key pair
    /// </summary>
    public class DsaKeyPair
    {
        /// <summary>
        /// Private Key X. Range [1, <see cref="PqgDomainParameters.Q"/> – 1]
        /// </summary>
        public BigInteger PrivateKeyX { get; }
        /// <summary>
        /// Public Key Y.  Range [1, <see cref="PqgDomainParameters.P"/> - 1]
        /// </summary>
        public BigInteger PublicKeyY { get; }

        public DsaKeyPair(BigInteger privateKeyX, BigInteger publicKeyY)
        {
            PrivateKeyX = privateKeyX;
            PublicKeyY = publicKeyY;
        }
    }
}