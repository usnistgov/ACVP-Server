using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.GGeneratorValidators;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class UnverifiableGeneratorGeneratorValidator : IGGeneratorValidator
    {
        private readonly IRandom800_90 _rand = new Random800_90();

        /// <summary>
        /// A.2.1
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="seed">Not needed</param>
        /// <param name="index">Not Needed</param>
        /// <returns></returns>
        public GGenerateResult Generate(BitString p, BitString q, DomainSeed seed = null, BitString index = null)
        {
            var pInt = p.ToPositiveBigInteger();
            var qInt = q.ToPositiveBigInteger();
            
            // 1 (always an integer)
            var e = (pInt - 1) / qInt;

            BigInteger h = 1;
            BigInteger g;
            do
            {
                // 2
                h++;
                if (h >= pInt - 1)
                {
                    // You couldn't feasibly reach this anyways... 
                    return new GGenerateResult("Too many h iterations");
                }

                // 3
                g = BigInteger.ModPow(h, e, pInt);

            // 4
            } while (g == 1);

            return new GGenerateResult(new BitString(g).PadToModulusMsb(32), new BitString(h));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="g"></param>
        /// <param name="seed">Not needed</param>
        /// <param name="index">Not needed</param>
        /// <returns></returns>
        public GValidateResult Validate(BitString p, BitString q, BitString g, DomainSeed seed = null, BitString index = null)
        {
            var pInt = p.ToPositiveBigInteger();
            var qInt = q.ToPositiveBigInteger();
            var gInt = g.ToPositiveBigInteger();
            
            // 1
            if (2 > gInt || gInt > pInt - 1)
            {
                return new GValidateResult("g not in required range");
            }

            // 2
            if (BigInteger.ModPow(gInt, qInt, pInt) != 1)
            {
                return new GValidateResult("g ^ q mod p != 1, invalid generator");
            }

            return new GValidateResult();
        }
    }
}
