using System.Numerics;
using NIST.CVP.Crypto.DSA.FCC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    /// <summary>
    /// The FFC implementation of DiffieHellman
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1
    /// </summary>
    public class FfcDiffieHellman : IDiffieHellman<FfcDomainParameters>
    {
        public DiffieHellmanResponse GenerateSharedSecretZ(
            FfcDomainParameters domainParameters, 
            BigInteger xPrivateKeyPartyA,
            BigInteger yPublicKeyPartyB)
        {

            var z = BigInteger.ModPow(yPublicKeyPartyB, xPrivateKeyPartyA, domainParameters.P);

            if (z == BigInteger.One)
            {
                return new DiffieHellmanResponse($"{nameof(z)} was 1, error.");
            }

            return new DiffieHellmanResponse(new BitString(z, 0, false));
        }
    }
}