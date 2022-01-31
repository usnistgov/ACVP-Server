using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.ECC
{
    public class DeterministicNonceProvider : IEccNonceProvider
    {
        private readonly IHmac _hmac;
        private readonly BitString _01bits = new BitString("01010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101010101");    // Maximum possible length (512 bits)

        public DeterministicNonceProvider(IHmac hmac)
        {
            _hmac = hmac;
        }

        // Question: Is it a hard restriction that the hash within HMAC is the same used within the signature process?
        public BigInteger GetNonce(BigInteger privateD, BigInteger hashedMessage, BigInteger orderN)
        {
            // 1.3
            var seedMaterial = new BitString(privateD).PadToModulusMsb(32).ConcatenateBits(new BitString(hashedMessage).PadToModulusMsb(32));

            // 1.4
            var key = BitString.Zeroes(_hmac.OutputLength);

            // 1.5
            var v = _01bits.GetMostSignificantBits(_hmac.OutputLength);

            // 1.6
            key = _hmac.Generate(key, v.ConcatenateBits(BitString.ConcatenateBits(BitString.Zeroes(8), seedMaterial))).Mac;

            // 1.7
            v = _hmac.Generate(key, v).Mac;

            // 1.8
            key = _hmac.Generate(key, v.ConcatenateBits(BitString.ConcatenateBits(new BitString("01"), seedMaterial))).Mac;

            // 1.9
            v = _hmac.Generate(key, v).Mac;

            // 2
            var nlen = orderN.ExactBitLength();

            // 3
            BigInteger k = 0;

            while (k == 0 || k >= orderN)
            {
                // 4.1
                var tmp = new BitString(0);

                // 4.2
                while (tmp.BitLength < nlen)
                {
                    v = _hmac.Generate(key, v).Mac;
                    tmp = tmp.ConcatenateBits(v);
                }

                // 4.3
                k = tmp.GetMostSignificantBits(nlen).ToPositiveBigInteger();

                // 4.4
                if (k > 0 && k < orderN)
                {
                    return k;
                }

                // 4.5
                key = _hmac.Generate(key, v.ConcatenateBits(BitString.Zeroes(8))).Mac;

                // 4.6
                v = _hmac.Generate(key, v).Mac;
            }

            return k;
        }
    }
}
