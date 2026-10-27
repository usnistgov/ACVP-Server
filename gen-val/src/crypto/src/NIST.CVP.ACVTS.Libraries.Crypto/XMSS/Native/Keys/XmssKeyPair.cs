using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys;

namespace NIST.CVP.ACVTS.Libraries.Crypto.XMSS.Native.Keys
{
    public record XmssKeyPair : IXmssKeyPair
    {
        public XmssAttribute XmssAttribute { get; init; }
        public IXmssPrivateKey PrivateKey { get; init; }
        public IXmssPublicKey PublicKey { get; init; }
    }
}
