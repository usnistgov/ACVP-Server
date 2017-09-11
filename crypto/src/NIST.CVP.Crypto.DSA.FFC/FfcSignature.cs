using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class FfcSignature : IDsaSignature
    {
        public BigInteger R { get; private set; }
        public BigInteger S { get; private set; }

        public FfcSignature(BigInteger r, BigInteger s)
        {
            R = r;
            S = s;
        }
    }
}
