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

        // FIPS 186-5, A.3.3 Per-Message Secret Number Generation for Deterministic ECDSA
        // Question: Is it a hard restriction that the hash within HMAC is the same used within the signature process?
        public BigInteger GetNonce(BigInteger privateD, BitString hashedMessage, BigInteger orderN)
        {
            var nLen = orderN.ExactBitLength();
            
            // 1.1 "Convert the private key d to an octet string using the procedure in Appendix B.2.3"
            // Note: from the FIPS, it's not clear what the length of the octet string produced by B.2.3 should be. But 
            // RFC 6979 Section 2.3.3 makes it clear that B.2.3 should produce a string rlen bits in length,
            // where rlen = 8 * ceil(qlen/8); qlen = len(n) => rlen = 8 * ceil( len(n)/8 )
            var rLen = (int) (8 * System.Math.Ceiling((double)nLen / 8)); 
            // 1. by definition, d is an integer and 0 < d < n
            // 2. the BitString constructor will always produce a byte-aligned string
            // 3. depending on the value of d, its BitString representation may be as small as a single byte.
            // 4. Do: convert d to a BitString and left-pad it with 0 bits until it is rlen bits in length
            var privateDOctets = new BitString(privateD).PadToModulusMsb(rLen);
            
            // 1.2 "Convert the Hash H to an octet string using the procedure in Appendix B.2.4 with modulus n."
            //  B.2.4 #1
            BitString nLenHashedMessage;
            if (hashedMessage.BitLength < nLen)
                nLenHashedMessage = BitString.Zeroes(nLen - hashedMessage.BitLength).ConcatenateBits(hashedMessage);
            else
                nLenHashedMessage = hashedMessage.GetMostSignificantBits(nLen);
            //  B.2.4 #2
            var c = nLenHashedMessage.ToPositiveBigInteger();
            //  B.2.4 #3
            if (c > orderN) c %= orderN;
            //  B.2.4 #4 & #5
            var hashedMessageOctets = new BitString(c).PadToModulusMsb(rLen); 
            
            // 1.3
            var seedMaterial = privateDOctets.ConcatenateBits(hashedMessageOctets);

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
            // var nLen = orderN.ExactBitLength();

            // 3
            BigInteger k = 0;

            while (k == 0 || k >= orderN)
            {
                // 4.1
                var tmp = new BitString(0);

                // 4.2
                while (tmp.BitLength < nLen)
                {
                    v = _hmac.Generate(key, v).Mac;
                    tmp = tmp.ConcatenateBits(v);
                }

                // 4.3
                k = tmp.GetMostSignificantBits(nLen).ToPositiveBigInteger();

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
