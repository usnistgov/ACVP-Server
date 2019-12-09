using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC
{
    public interface IEccNonceProviderFactory
    {
        IEccNonceProvider GetNonceProvider(NonceProviderTypes nonceTypes, IHmac hmac, IEntropyProvider entropyProvider);
    }
}