using System.Numerics;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    /// <inheritdoc />
    /// <summary>
    /// The FFC implementation of DiffieHellman
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1
    /// </summary>
    public class DiffieHellman : IDiffieHellman
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            BigInteger p, 
            BigInteger xPrivateKeyPartyA,
            BigInteger yPublicKeyPartyB)
        {

            var z = new BitString(BigInteger.ModPow(yPublicKeyPartyB, xPrivateKeyPartyA, p));

            if (z.ToPositiveBigInteger() == 1)
            {
                return new SharedSecretResponse($"{nameof(z)} was 1, error.");
            }

            if (z.BitLength % 32 != 0)
            {
                z = BitString.ConcatenateBits(BitString.Zeroes(32 - z.BitLength % 32), z);
            }

            return new SharedSecretResponse(z);
        }
    }
}