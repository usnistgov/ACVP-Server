using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.RSA2.PrimeGenerators
{
    public class AuxiliaryResult
    {
        public BigInteger XP { get; set; }
        public BigInteger XQ { get; set; }
        public BigInteger XP1 { get; set; }
        public BigInteger XQ1 { get; set; }
        public BigInteger XP2 { get; set; }
        public BigInteger XQ2 { get; set; }
    }
}
