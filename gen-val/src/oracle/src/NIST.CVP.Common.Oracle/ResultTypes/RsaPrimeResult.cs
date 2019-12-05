using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaPrimeResult : IResult
    {
        public bool Success { get; set; }
        public KeyPair Key { get; set; }
        public AuxiliaryResult Aux { get; set; }
    }
}
