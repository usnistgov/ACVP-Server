using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Helpers
{
    public class CurveAttributes
    {
        public CurveAttributes(Curve curveName, CurveType curveType, int degreeOfPolynomial, int exactBitLengthOrderN)
        {
            CurveName = curveName;
            CurveType = curveType;
            DegreeOfPolynomial = degreeOfPolynomial;
            ExactBitLengthOrderN = exactBitLengthOrderN;
        }

        public Curve CurveName { get; }
        public CurveType CurveType { get; }
        public int DegreeOfPolynomial { get; }
        public int ExactBitLengthOrderN { get; }
    }
}
