using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.DSA.ECC.Enums;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public interface IEccCurveFactory
    {
        EccCurveBase GetCurve(Curve curve);
    }
}
