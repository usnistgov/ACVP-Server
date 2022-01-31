using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccCurveFactory
    {
        IEccCurve GetCurve(Curve curve);
    }
}
