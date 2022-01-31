using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Helpers
{
    public class CurveAttributes
    {
        public CurveAttributes(Curve curveName, int lengthN)
        {
            CurveName = curveName;
            LengthN = lengthN;
        }

        public Curve CurveName { get; }
        public int LengthN { get; }
    }
}
