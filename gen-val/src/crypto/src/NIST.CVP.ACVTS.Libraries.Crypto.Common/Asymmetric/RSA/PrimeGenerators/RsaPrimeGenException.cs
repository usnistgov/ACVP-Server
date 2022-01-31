using System;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public class RsaPrimeGenException : Exception
    {
        public RsaPrimeGenException(string error) : base(error) { }
    }
}
