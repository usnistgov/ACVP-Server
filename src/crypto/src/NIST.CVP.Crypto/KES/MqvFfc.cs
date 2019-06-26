using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.KES;
using NIST.CVP.Crypto.KES.Helpers;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    /// <inheritdoc />
    /// <summary>
    /// Describes the methods for FFC MQV key establishment scheme
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1 (FFC)
    /// </summary>
    public class MqvFfc : IMqv<FfcDomainParameters, FfcKeyPair>
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            FfcDomainParameters domainParameters,
            FfcKeyPair xPrivateStaticKeyPartyA,
            FfcKeyPair yPublicStaticKeyPartyB,
            FfcKeyPair rPrivateKeyPartyA,
            FfcKeyPair tPublicKeyPartyA,
            FfcKeyPair tPublicKeyPartyB)
        {
            // 1. w = ceil (len(q) / 2)
            var qBitString = domainParameters.Q;
            int lenQ = qBitString.BitLength;
            int w = lenQ / 2 + ((lenQ % 2 != 0) ? 1 : 0);

            // 2^w is equal to (1 << w)
            var tw = BigInteger.One << w;

            // 2. T_A = t_A mod 2^w + 2^w
            var T_A = (tPublicKeyPartyA.PublicKeyY.ToPositiveBigInteger() % tw) + tw;

            // 3. S_A = (r_A + T_A x_A) mod q
            var S_A = (rPrivateKeyPartyA.PrivateKeyX.ToPositiveBigInteger() + (T_A * xPrivateStaticKeyPartyA.PrivateKeyX.ToPositiveBigInteger())) % domainParameters.Q.ToPositiveBigInteger();

            // 4. T_B = (t_B mod 2^w) + 2^w
            var T_B = (tPublicKeyPartyB.PublicKeyY.ToPositiveBigInteger() % tw) + tw;

            // 5. Z = ((t_B * (y_B^T_B))^S_A) mod p
            // Two steps: 1. me1 = y_B ^ T_B mod p   2. z = (t_B * me1) ^ S_A mod p
            var me1 = BigInteger.ModPow(yPublicStaticKeyPartyB.PublicKeyY.ToPositiveBigInteger(), T_B, domainParameters.P.ToPositiveBigInteger());
            var z = new BitString(BigInteger.ModPow((tPublicKeyPartyB.PublicKeyY.ToPositiveBigInteger() * me1), S_A, domainParameters.P.ToPositiveBigInteger()));

            // 6 if z = 1, fail
            if (z.ToPositiveBigInteger() == 1)
            {
                return new SharedSecretResponse($"{nameof(z)} was 1, error.");
            }

            SharedSecretZHelper.FormatFfcSharedSecretZ(ref z);

            return new SharedSecretResponse(z);
        }
    }
}