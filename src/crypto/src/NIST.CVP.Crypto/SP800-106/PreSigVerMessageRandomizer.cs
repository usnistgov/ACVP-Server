using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using System;

namespace NIST.CVP.Crypto
{
    public class PreSigVerMessageRandomizer : IPreSigVerMessageRandomizer
    {
        private readonly IEntropyProvider _entropyProvider;

        public PreSigVerMessageRandomizer(IEntropyProvider entropyProvider)
        {
            _entropyProvider = entropyProvider;
        }

        public BitString RandomizeMessage(BitString message, int randomizationSecurityStrength)
        {
            var rv = _entropyProvider.GetEntropy(randomizationSecurityStrength);
            BitString padding = BitString.One();

            // from https://nvlpubs.nist.gov/nistpubs/Legacy/SP/nistspecialpublication800-106.pdf
            /*
            1. If(| Ms | ≥ (| rv | -1))
            {
                1.1 padding = 1.
            }
            Else
            {
                1.2 padding = 1 || 0(| rv | - | Ms | -1).
            }
            */
            if (message.BitLength < rv.BitLength - 1)
            {
                padding = padding.ConcatenateBits(BitString.Zeroes(rv.BitLength - message.BitLength - 1));
            }

            // 2. m = Ms || padding.
            var m = message.ConcatenateBits(padding);

            // 3. n is a positive integer, and n = | rv |.
            var n = rv.BitLength;

            // 4. If(n > 1024) then stop and output an error indicator(see Section 3.3).
            if (n < 80 || n > 1024)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            // 5. counter = ⎣| m | / n⎦.
            var counter = (int)System.Math.Floor((double)m.BitLength / n);

            // 6. remainder = (| m | mod n).
            var remainder = m.BitLength % n;

            /*
            7. Concatenate counter copies of the rv to the remainder left - most bits of the rv to get Rv,
                such that | Rv | = | m |.
            */
            var Rv = new BitString(0);
            for (var i = 0; i < counter; i++)
            {
                Rv = Rv.ConcatenateBits(rv);
            }
            Rv = Rv.ConcatenateBits(rv.GetLeastSignificantBits(remainder));

            // Sanity check
            if (Rv.BitLength != m.BitLength)
            {
                throw new ArgumentOutOfRangeException(nameof(Rv));
            }

            /*
            8. Convert n to a 16 - bit binary string rv_length_indicator using the
                rv_length_indicator_generation function specified in the Appendix.
                rv_length_indicator = rv_length_indicator_generation(n).
            */
            // this cast should be safe as we're ensuring n <= 1024
            var nBitString = BitString.To16BitString((short)n);

            //9. M = rv || (m ⊕ Rv) || rv_length_indicator(Figure 1).
            return rv
                .ConcatenateBits(m.XOR(Rv))
                .ConcatenateBits(nBitString);
        }
    }
}