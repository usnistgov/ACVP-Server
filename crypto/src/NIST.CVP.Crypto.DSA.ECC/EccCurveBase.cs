using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public abstract class EccCurveBase
    {
        public abstract EccPoint ComputeY(EccPoint xPoint);
    }
}
