using System.Numerics;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FCC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    /// Describes the methods for FFC MQV key establishment scheme
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1 (FFC)
    /// <typeparam name="TDsaDomainParameters">The <see cref="IDsa"/> Domain Parameters</typeparam>
    public class FfcMqv : IMqv<FfcDomainParameters>
    {
        public DiffieHellmanResponse GenerateSharedSecretZ(
            FfcDomainParameters domainParameters, 
            BigInteger xPrivateStaticKeyPartyA,
            BigInteger yPublicStaticKeyPartyB, 
            BigInteger rPrivateKeyPartyA, 
            BigInteger tPublicKeyPartyA,
            BigInteger tPublicKeyPartyB)
        {
            // 1. w = ceil (len(q) / 2)
            var qBitString = new BitString(domainParameters.Q, 0, true);
            int lenQ = qBitString.BitLength;
            int w = lenQ / 2 + ((lenQ % 2 != 0) ? 1 : 0);

            // 2^w is equal to (1 << w)
            var tw = BigInteger.One << w;

            // 2. T_A = t_A mod 2^w + 2^w
            var T_A = (tPublicKeyPartyA % tw) + tw;

            // 3. S_A = (r_A + T_A x_A) mod q
            var S_A = (rPrivateKeyPartyA + (T_A * xPrivateStaticKeyPartyA)) % domainParameters.Q;

            // 4. T_B = (t_B mod 2^w) + 2^w
            var T_B = (tPublicKeyPartyB % tw) + tw;

            // 5. Z = ((t_B * (y_B^T_B))^S_A) mod p
            // Two steps: 1. me1 = y_B ^ T_B mod p   2. z = (t_B * me1) ^ S_A mod p
            var me1 = BigInteger.ModPow(yPublicStaticKeyPartyB, T_B, domainParameters.P);
            var z = BigInteger.ModPow((tPublicKeyPartyB * me1), S_A, domainParameters.P);

            return new DiffieHellmanResponse(new BitString(z, 0, false));
        }
    }
}