using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
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

        public GGenerateResult Generate(BitString p, BitString q, DomainSeed seed = null, BitString index = null)
        {
            // 1
            if (index.BitLength != 8)
            {
                return new GGenerateResult("Invalid index");
            }

            var pInt = p.ToPositiveBigInteger();
            var qInt = q.ToPositiveBigInteger();
            
            // 3
            var e = (pInt - 1) / qInt;

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
                var countBS = new BitString(count, 16, false);      // value must be 16 bits long
                var U = BitString.ConcatenateBits(seed.GetFullSeed(), _ggen);
                U = BitString.ConcatenateBits(U, index);
                U = BitString.ConcatenateBits(U, countBS);

                // 8, 9
                var W = _sha.HashMessage(U).ToBigInteger();
                g = BigInteger.ModPow(W, e, pInt);

                // 10
            } while (g < 2);

            // 11
            return new GGenerateResult(new BitString(g).PadToModulusMsb(32));
        }

        public GValidateResult Validate(BitString p, BitString q, BitString g, DomainSeed seed = null, BitString index = null)
        {
            // 1
            if (index.BitLength != 8)
            {
                return new GValidateResult("Invalid index");
            }

            var gInt = g.ToPositiveBigInteger();
            var qInt = q.ToPositiveBigInteger();
            var pInt = p.ToPositiveBigInteger();
            
            // 2
            if (2 > gInt || gInt > pInt - 1)
            {
                return new GValidateResult("Invalid generator value, out of range");
            }

            // 3
            if(BigInteger.ModPow(gInt, qInt, pInt) != 1)
            {
                return new GValidateResult("Invalid generator value, g^q % p != 1");
            }

            // 5
            var e = (pInt - 1) / qInt;

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
                var countBS = new BitString(count, 16, false);      // value must be 16 bits long
                var U = BitString.ConcatenateBits(seed.GetFullSeed(), _ggen);
                U = BitString.ConcatenateBits(U, index);
                U = BitString.ConcatenateBits(U, countBS);
                
                // 10
                var W = _sha.HashMessage(U).ToBigInteger();

                // 11
                computed_g = BigInteger.ModPow(W, e, pInt);

                //12
            } while (computed_g < 2);

            // 13
            if (computed_g != gInt)
            {
                return new GValidateResult("Incorrect g value obtained");
            }

            return new GValidateResult();
        }
    }
}
