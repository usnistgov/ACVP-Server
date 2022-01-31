using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KES;
using NIST.CVP.ACVTS.Libraries.Crypto.KES.Helpers;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KES
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
                    yPublicKeyPartyB.PublicKeyY,
                    xPrivateKeyPartyA.PrivateKeyX,
                    domainParameters.P
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
