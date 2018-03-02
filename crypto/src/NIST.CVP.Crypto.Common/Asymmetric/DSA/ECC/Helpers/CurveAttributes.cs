using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Helpers
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