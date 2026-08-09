using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Helpers;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native.Keys
{
    public interface IXmssKeyPair
    {
        XmssAttribute XmssAttribute { get; }
        IXmssPrivateKey PrivateKey { get; }
        IXmssPublicKey PublicKey { get; }
    }
}
