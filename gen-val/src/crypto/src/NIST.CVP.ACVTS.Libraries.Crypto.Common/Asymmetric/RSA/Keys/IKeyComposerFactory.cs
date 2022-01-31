using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys
{
    public interface IKeyComposerFactory
    {
        IRsaKeyComposer GetKeyComposer(PrivateKeyModes privateKeyModes);
    }
}
