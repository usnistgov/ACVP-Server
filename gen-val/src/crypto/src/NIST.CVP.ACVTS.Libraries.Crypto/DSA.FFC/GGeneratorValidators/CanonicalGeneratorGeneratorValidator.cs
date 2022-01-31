using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC.GGeneratorValidators
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

                // Need to make sure the BigInteger conversion doesn't drop leading 00 bytes, always have as much seed as q
                var seedBits = new BitString(seed.GetFullSeed()).PadToModulusMsb(32);

                // 7
                var countBS = new BitString(count, 16, false);      // value must be 16 bits long
                var U = BitString.ConcatenateBits(seedBits, _ggen);
                U = BitString.ConcatenateBits(U, index);
                U = BitString.ConcatenateBits(U, countBS);

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
            if (2 > g || g > p - 1)
            {
                return new GValidateResult("Invalid generator value, out of range");
            }

            // 3
            if (BigInteger.ModPow(g, q, p) != 1)
            {
                return new GValidateResult("Invalid generator value, g^q % p != 1");
            }

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

                // Need to make sure the BigInteger conversion doesn't drop leading 00 bytes, always have as much seed as q
                var seedBits = new BitString(seed.GetFullSeed()).PadToModulusMsb(32);

                // 9
                var countBS = new BitString(count, 16, false);      // value must be 16 bits long
                var U = BitString.ConcatenateBits(seedBits, _ggen);
                U = BitString.ConcatenateBits(U, index);
                U = BitString.ConcatenateBits(U, countBS);

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
