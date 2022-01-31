using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed
{
    public interface IEdwardsCurveFactory
    {
        IEdwardsCurve GetCurve(Curve curve);
    }
}
