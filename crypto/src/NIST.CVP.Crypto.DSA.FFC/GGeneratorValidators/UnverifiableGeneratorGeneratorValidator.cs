using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC.GGeneratorValidators
{
    public class UnverifiableGeneratorGeneratorValidator : IGGeneratorValidator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="seed">Not needed</param>
        /// <param name="index">Not Needed</param>
        /// <returns></returns>
        public GGenerateResult Generate(BigInteger p, BigInteger q, DomainSeed seed = null, int index = 0)
        {
            throw new NotImplementedException();
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
        public GValidateResult Validate(BigInteger p, BigInteger q, BigInteger g, DomainSeed seed = null, int index = 0)
        {
            throw new NotImplementedException();
        }
    }
}
