using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccCurveFactory
    {
        IEccCurve GetCurve(Curve curve);
    }
}
