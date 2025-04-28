using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH.Enums;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XECDH
{
    public interface IXecdhFactory
    {
        IXecdh GetXecdh(Curve curve, IEntropyProvider entropyProvider);

        IXecdh GetXecdh(Curve curve, EntropyProviderTypes entropyType = EntropyProviderTypes.Random);
    }
}
