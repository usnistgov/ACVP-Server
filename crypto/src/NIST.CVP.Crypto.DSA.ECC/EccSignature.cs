using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class EccSignature : IDsaSignature
    {
        public BigInteger R { get; }
        public BigInteger S { get; }

        public EccSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
