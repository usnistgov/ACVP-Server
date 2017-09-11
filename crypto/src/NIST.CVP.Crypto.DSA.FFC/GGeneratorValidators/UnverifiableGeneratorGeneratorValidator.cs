using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
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
        public GGenerateResult Generate(BigInteger p, BigInteger q, DomainSeed seed = null, BitString index = null)
        {
            // 1 (always an integer)
            var e = (p - 1) / q;

            // 2
            var h = _rand.GetRandomBigInteger(2, p - 2);

            // 3
            BigInteger g;
            do
            {
                g = BigInteger.ModPow(h, e, p);

            // 4
            } while (g == 1);

            return new GGenerateResult(g);
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
        public GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g, DomainSeed seed = null, BitString index = null)
        {
            // 1
            if (2 <= g && g <= p - 1)
            {
                return new GValidateResult("g not in required range");
            }

            // 2
            if (BigInteger.ModPow(g, q, p) != 1)
            {
                return new GValidateResult("g ^ q mod p != 1, invalid generator");
            }

            return new GValidateResult();
        }
    }
}
