using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccNonceProviderFactory
    {
        IEccNonceProvider GetNonceProvider(NonceProviderTypes nonceTypes, IHmac hmac, IEntropyProvider entropyProvider);
    }
}
