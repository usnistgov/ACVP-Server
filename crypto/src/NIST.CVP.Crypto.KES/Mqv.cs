using System.Numerics;
using NIST.CVP.Crypto.DSA;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KES
{
    /// Describes the methods for FFC MQV key establishment scheme
    /// 
    /// http://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-56Ar2.pdf
    /// Section 5.7.1.1 (FFC)
    public class Mqv : IMqv
    {
        public SharedSecretResponse GenerateSharedSecretZ(
            BigInteger p,
            BigInteger q, 
            BigInteger xPrivateStaticKeyPartyA,
            BigInteger yPublicStaticKeyPartyB, 
            BigInteger rPrivateKeyPartyA, 
            BigInteger tPublicKeyPartyA,
            BigInteger tPublicKeyPartyB)
        {
            // 1. w = ceil (len(q) / 2)
            var qBitString = new BitString(q);
            int lenQ = qBitString.BitLength;
            int w = lenQ / 2 + ((lenQ % 2 != 0) ? 1 : 0);

            // 2^w is equal to (1 << w)
            var tw = BigInteger.One << w;

            // 2. T_A = t_A mod 2^w + 2^w
            var T_A = (tPublicKeyPartyA % tw) + tw;

            // 3. S_A = (r_A + T_A x_A) mod q
            var S_A = (rPrivateKeyPartyA + (T_A * xPrivateStaticKeyPartyA)) % q;

            // 4. T_B = (t_B mod 2^w) + 2^w
            var T_B = (tPublicKeyPartyB % tw) + tw;

            // 5. Z = ((t_B * (y_B^T_B))^S_A) mod p
            // Two steps: 1. me1 = y_B ^ T_B mod p   2. z = (t_B * me1) ^ S_A mod p
            var me1 = BigInteger.ModPow(yPublicStaticKeyPartyB, T_B, p);
            var z = new BitString(BigInteger.ModPow((tPublicKeyPartyB * me1), S_A, p));

            if (z.BitLength % 32 != 0)
            {
                z = BitString.ConcatenateBits(BitString.Zeroes(32 - z.BitLength % 32), z);
            }

            return new SharedSecretResponse(z);
        }
    }
}