using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IEdwardsCurveFactory
    {
        IEdwardsCurve GetCurve(Curve curve);
    }
}
