using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class CanonicalGeneratorGeneratorValidator : IGGeneratorValidator
    {
        private readonly ISha _sha;
        private readonly BitString _ggen = new BitString("6767656E"); // literally 'ggen' in ascii hex

        public CanonicalGeneratorGeneratorValidator(ISha sha)
        {
            _sha = sha;
        }

        public GGenerateResult Generate(BigInteger p, BigInteger q, DomainSeed seed = null, BitString index = null)
        {
            // 1
            if (index.BitLength != 8)
            {
                return new GGenerateResult("Invalid index");
            }

            // 2
            var N = new BitString(q).BitLength;

            // 3
            var e = (p - 1) / q;

            // 4
            var count = 0;
            BigInteger g;
            do
            {
                // 5
                count++;

                // 6
                if (count == 0)
                {
                    return new GGenerateResult("Invalid count");
                }

                // 7
                var U = BitString.ConcatenateBits(new BitString(seed.GetFullSeed()), _ggen);
                U = BitString.ConcatenateBits(U, index);
                U = BitString.ConcatenateBits(U, new BitString((BigInteger)count));

                // 8, 9
                var W = _sha.HashMessage(U).ToBigInteger();
                g = BigInteger.ModPow(W, e, p);

                // 10
            } while (g < 2);

            // 11
            return new GGenerateResult(g);
        }

        public GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g, DomainSeed seed = null, BitString index = null)
        {
            // 1
            if (index.BitLength != 8)
            {
                return new GValidateResult("Invalid index");
            }

            // 2
            if (2 <= g && g <= p - 1)
            {
                return new GValidateResult("Invalid generator value, out of range");
            }

            // 3
            if(BigInteger.ModPow(g, q, p) != 1)
            {
                return new GValidateResult("Invalid generator value, g^q % p != 1");
            }

            // 4
            var N = new BitString(q).BitLength;

            // 5
            var e = (p - 1) / q;

            // 6
            var count = 0;
            BigInteger computed_g;

            // 7
            do
            {
                count++;

                // 8
                if (count == 0)
                {
                    return new GValidateResult("Too many iterations to find g");
                }

                // 9
                var U = BitString.ConcatenateBits(new BitString(seed.GetFullSeed()), _ggen);
                U = BitString.ConcatenateBits(U, index);
                U = BitString.ConcatenateBits(U, new BitString((BigInteger)count));
                
                // 10
                var W = _sha.HashMessage(U).ToBigInteger();

                // 11
                computed_g = BigInteger.ModPow(W, e, p);

                //12
            } while (computed_g < 2);

            // 13
            if (computed_g != g)
            {
                return new GValidateResult("Incorrect g value obtained");
            }

            return new GValidateResult();
        }
    }
}
