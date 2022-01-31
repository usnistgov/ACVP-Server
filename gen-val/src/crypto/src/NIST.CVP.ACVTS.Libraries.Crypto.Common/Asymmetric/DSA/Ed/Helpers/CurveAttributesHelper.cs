using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Helpers
{
    public static class CurveAttributesHelper
    {
        public static Dictionary<Curve, CurveAttributes> CurveAttributes = new Dictionary<Curve, CurveAttributes>()
        {
            {Curve.Ed25519, new CurveAttributes(Curve.Ed25519, 192)}, // need to fix length N
            {Curve.Ed448, new CurveAttributes(Curve.Ed448,  224)} // same here
        };

        public static CurveAttributes GetCurveAttribute(Curve curve)
        {
            return CurveAttributes.First(w => w.Key == curve).Value;
        }
    }
}
