using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class RsaPrimeResult : IResult
    {
        public bool Success { get; set; }
        public KeyPair Key { get; set; }
        public AuxiliaryResult Aux { get; set; }
    }
}
