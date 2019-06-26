using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.KES.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    /// <inheritdoc />
    /// <summary>
    /// The FFC implementation of DiffieHellman
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1
    /// </summary>
    public class DiffieHellmanFfc : IDiffieHellman<FfcDomainParameters, FfcKeyPair>
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            FfcDomainParameters domainParameters,
            FfcKeyPair xPrivateKeyPartyA,
            FfcKeyPair yPublicKeyPartyB)
        {

            var z = new BitString(
                BigInteger.ModPow(
                    yPublicKeyPartyB.PublicKeyY.ToPositiveBigInteger(),
                    xPrivateKeyPartyA.PrivateKeyX.ToPositiveBigInteger(),
                    domainParameters.P.ToPositiveBigInteger()
                )
            );

            if (z.ToPositiveBigInteger() == 1)
            {
                return new SharedSecretResponse($"{nameof(z)} was 1, error.");
            }

            SharedSecretZHelper.FormatFfcSharedSecretZ(ref z);

            return new SharedSecretResponse(z);
        }
    }
}