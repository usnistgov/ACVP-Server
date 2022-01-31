using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers
{
    public static class CurveAttributesHelper
    {
        public readonly static Dictionary<Curve, CurveAttributes> CurveAttributes = new Dictionary<Curve, CurveAttributes>()
        {
            {Curve.P192, new CurveAttributes(Curve.P192, CurveType.Prime, 192, 192)},
            {Curve.P224, new CurveAttributes(Curve.P224, CurveType.Prime, 224, 224)},
            {Curve.P256, new CurveAttributes(Curve.P256, CurveType.Prime, 256, 256)},
            {Curve.P384, new CurveAttributes(Curve.P384, CurveType.Prime, 384, 384)},
            {Curve.P521, new CurveAttributes(Curve.P521, CurveType.Prime, 521, 521)}, // Should be 521 not 512.
            {Curve.K163, new CurveAttributes(Curve.K163, CurveType.Binary, 163, 163)},
            {Curve.K233, new CurveAttributes(Curve.K233, CurveType.Binary, 233, 232)},
            {Curve.K283, new CurveAttributes(Curve.K283, CurveType.Binary, 283, 281)},
            {Curve.K409, new CurveAttributes(Curve.K409, CurveType.Binary, 409, 407)},
            {Curve.K571, new CurveAttributes(Curve.K571, CurveType.Binary, 571, 570)},
            {Curve.B163, new CurveAttributes(Curve.B163, CurveType.Binary, 163, 163)},
            {Curve.B233, new CurveAttributes(Curve.B233, CurveType.Binary, 233, 233)},
            {Curve.B283, new CurveAttributes(Curve.B283, CurveType.Binary, 283, 282)},
            {Curve.B409, new CurveAttributes(Curve.B409, CurveType.Binary, 409, 409)},
            {Curve.B571, new CurveAttributes(Curve.B571, CurveType.Binary, 571, 570)}
        };

        public static CurveAttributes GetCurveAttribute(Curve curve) =>
            CurveAttributes.First(w => w.Key == curve).Value;
    }
}
