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

            var z = BigInteger.ModPow(yPublicKeyPartyB, xPrivateKeyPartyA, p);

            if (z == BigInteger.One)
            {
                return new SharedSecretResponse($"{nameof(z)} was 1, error.");
            }

            return new SharedSecretResponse(new BitString(z));
        }
    }
}