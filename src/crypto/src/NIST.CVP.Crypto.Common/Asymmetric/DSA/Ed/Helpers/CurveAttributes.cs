using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Helpers
{
    public class CurveAttributes
    {
        public CurveAttributes(Curve curveName, CurveType curveType, int lengthN)
        {
            CurveName = curveName;
            CurveType = curveType;
            LengthN = lengthN;
        }

        public Curve CurveName { get; }
        public CurveType CurveType { get; }
        public int LengthN { get; }
    }
}